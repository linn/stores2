namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class SuReqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        public SuReqCreationStrategy(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            IRepository<Employee, int> employeeRepository,
            IRepository<StorageLocation, int> storageLocationRepository
        )
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.employeeRepository = employeeRepository;
            this.storageLocationRepository = storageLocationRepository;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.GetRequisitionActionByFunction("SUREQ"),
                    privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to raise SUREQ");
            }

            var who = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);

            var fromLocation = string.IsNullOrEmpty(context.FromLocationCode)
                ? null
                : await this.storageLocationRepository
                    .FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = string.IsNullOrEmpty(context.ToLocationCode)
                ? null
                : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.ToLocationCode);

            await this.requisitionManager.Validate(
                context.CreatedByUserNumber,
                context.Function.FunctionCode,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                context.DepartmentCode,
                context.NominalCode,
                context.FirstLineCandidate,
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                context.FromLocationCode,
                context.ToLocationCode,
                context.PartNumber,
                context.Quantity,
                context.FromState,
                context.ToState,
                context.BatchRef,
                context.BatchDate,
                context.Document1Line);

            // header
            var req = new RequisitionHeader(
                who,
                context.Function,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                null,
                null,
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                fromLocation,
                toLocation,
                null,
                context.Quantity,
                context.Document1Line,
                context.FromState,
                context.ToState);

            await this.repository.AddAsync(req);


            // lines
            try
            {
                foreach (var line in context.Lines)
                {
                    await this.requisitionManager.AddRequisitionLine(req, line);
                }
            }
            catch (DomainException ex)
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
                    throw new CreateRequisitionException(
                        cancelAlsoFailedMessage,
                        ex);
                }
            }

            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
