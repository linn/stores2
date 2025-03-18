namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class LinesProvidedStrategy : ICreationStrategy
    {
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IAuthorisationService authorisationService;

        private readonly ILog logger;

        public LinesProvidedStrategy(
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IAuthorisationService authorisationService,
            ILog logger)
        {
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.authorisationService = authorisationService;
            this.logger = logger;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges?.ToList();
            var authAction = AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode);
            if (!this.authorisationService.HasPermissionFor(authAction, privilegesList))
            {
                throw new UnauthorisedActionException($"You are not authorised to raise {context.Function.FunctionCode}");
            }

            var employee = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var department = await this.departmentRepository.FindByIdAsync(context.DepartmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(context.NominalCode);
            var fromLocation = await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.ToLocationCode);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);

            var req = new RequisitionHeader(
                employee,
                context.Function,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                department,
                nominal,
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                fromLocation,
                toLocation,
                part,
                context.Quantity,
                context.Document1Line,
                context.ToState,
                context.FromState,
                context.BatchRef,
                context.BatchDate);

            await this.repository.AddAsync(req);

            foreach (var lineCandidate in context.Lines)
            {
                try
                {
                    await this.requisitionManager.AddRequisitionLine(req, lineCandidate);
                }
                catch (DomainException ex)
                {
                    var createFailedMessage =
                        $"Req failed to create since line could not be added. Reason: {ex.Message}";

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
            }

            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
