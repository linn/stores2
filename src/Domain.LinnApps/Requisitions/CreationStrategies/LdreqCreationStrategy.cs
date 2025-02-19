namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class LdreqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly ILog logger;

        private readonly IRequisitionManager requisitionManager;

        public LdreqCreationStrategy(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            ILog logger)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.logger = logger;
        }

        public async Task Apply(
            RequisitionHeader requisition, 
            LineCandidate firstLine, 
            int creationBy, 
            IEnumerable<string> privileges)
        {
            var enumerable = privileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.Ldreq, enumerable))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            await this.repository.AddAsync(requisition);

            try
            {
                await this.requisitionManager.AddRequisitionLine(requisition, firstLine);
            }
            catch (Exception ex)
                when (ex is PickStockException or CreateNominalPostingException)
            {
                var createFailedMessage =
                    $"Req failed to create since first line could not be added. Reason: {ex.Message}";

                // Try to cancel the header if adding the line fails
                try
                {
                    await this.requisitionManager.CancelHeader(
                        requisition.ReqNumber,
                        creationBy,
                        enumerable,
                        createFailedMessage,
                        false);
                }
                catch (CancelRequisitionException x)
                {
                    var cancelAlsoFailedMessage =
                        $"Warning - req failed to create: {ex.Message}. Header also failed to cancel: {x.Message}. Some cleanup may be required!";
                    this.logger.Error(cancelAlsoFailedMessage);
                    throw new CreateRequisitionException(
                        cancelAlsoFailedMessage,
                        ex);
                }


                this.logger.Error(createFailedMessage);
                throw new CreateRequisitionException(
                    createFailedMessage,
                    ex);
            }

            requisition = await this.repository.FindByIdAsync(requisition.ReqNumber);
        }
    }
}

