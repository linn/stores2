namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

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

        public async Task<RequisitionHeader> Create(
           RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.Ldreq, privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            var req = new RequisitionHeader(
                new Employee(), // todo
                context.Function,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                new Department(),
                new Nominal(),
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                new StorageLocation(),
                new StorageLocation(),
               new Part(),
                context.Quantity,
                context.Document1Line,
                context.FromState,
                context.ToState);

            await this.repository.AddAsync(req);

            try
            {
                await this.requisitionManager.AddRequisitionLine(req, context.FirstLineCandidate);
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
                        req.ReqNumber,
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

            // reload the latest state since stored procedures will have ran and written data
            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}

