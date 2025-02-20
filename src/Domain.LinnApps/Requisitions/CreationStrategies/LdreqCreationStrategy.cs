namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class LdreqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly ILog logger;

        private readonly IRequisitionManager requisitionManager;

        // todo  - this will probably need renamed or refactored
        // when it becomes clear some of this code will not be specifically tied
        // to LDREQs
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
           RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.Ldreq, privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            var header = context.Header;

            if (context.FirstLineCandidate == null && header.Part == null)
            {
                throw new CreateRequisitionException(
                    "Cannot create - no line specified and header does not specify part");
            }

            await this.repository.AddAsync(header);

            // LDREQ from
            if (header.ReqType == "F")
            {
                try
                {
                    // no lines? - todo - can you do an ldreq from without lines? don't think so since need stock picks?
                    await this.requisitionManager.AddRequisitionLine(header, context.FirstLineCandidate);
                }
                catch (DomainException ex)
                    when (ex is PickStockException or CreateNominalPostingException)
                {
                    var createFailedMessage =
                        $"Req failed to create since first line could not be added. Reason: {ex.Message}";

                    // Try to cancel the header if adding the line fails
                    try
                    {
                        await this.requisitionManager.CancelHeader(
                            header.ReqNumber,
                            context.CreatedByUserNumber,
                            privilegesList,
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
            }

            // LDREQ onto
            if (header.ReqType == "O")
            {

            }


            // reload the latest state since stored procedures will have ran and written data
            context.Header = await this.repository.FindByIdAsync(header.ReqNumber);
        }
    }
}

