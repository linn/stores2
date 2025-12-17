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

    public class LdreqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;

        private readonly IRequisitionRepository repository;

        private readonly ILog logger;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository;

        public LdreqCreationStrategy(
            IAuthorisationService authService,
            IRequisitionRepository repository,
            IRequisitionManager requisitionManager,
            ILog logger,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.logger = logger;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.transactionDefinitionRepository = transactionDefinitionRepository;
        }

        public async Task<RequisitionHeader> Create(
           RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode), privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            var who = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var department = await this.departmentRepository.FindByIdAsync(context.DepartmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(context.NominalCode);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);

            var fromLocation = string.IsNullOrEmpty(context.FromLocationCode)
                ? null : await this.storageLocationRepository
                             .FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = string.IsNullOrEmpty(context.ToLocationCode)
                                   ? null : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.ToLocationCode);

            await this.requisitionManager.Validate(
                context.CreatedByUserNumber,
                context.Function.FunctionCode,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                context.DepartmentCode,
                context.NominalCode,
                reference: context.Reference,
                comments: context.Comments,
                manualPick: context.ManualPick,
                fromStockPool: context.FromStockPool,
                toStockPool: context.ToStockPool,
                fromPalletNumber: context.FromPallet,
                toPalletNumber: context.ToPallet,
                fromLocationCode: context.FromLocationCode,
                toLocationCode: context.ToLocationCode,
                partNumber: context.PartNumber,
                quantity: context.Quantity,
                fromState: context.FromState,
                toState: context.ToState,
                batchRef: context.BatchRef,
                batchDate: context.BatchDate,
                document1Line: context.Document1Line,
                lines: context.Lines);

            // header
            var req = new RequisitionHeader(
                who,
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
                context.FromState
                );

            await this.repository.AddAsync(req);

            // lines
            try
            {
                // todo: for each line
                foreach (var lineCandidate in context.Lines)
                {
                    
                    await this.requisitionManager.AddRequisitionLine(req, lineCandidate);
                }
                
                // all the lines have the same TransactionDefinition
                // so just take from the first one
                var transactionDefinition = await this.transactionDefinitionRepository
                                                .FindByIdAsync(context.Lines.First().TransactionDefinition);
                req.SetStateAndCategory(
                    req.FromState ?? transactionDefinition.FromState, 
                    req.ToState ?? transactionDefinition.InspectedState, 
                    transactionDefinition.OntoCategory,
                    transactionDefinition.FromCategory); 
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
           
            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
