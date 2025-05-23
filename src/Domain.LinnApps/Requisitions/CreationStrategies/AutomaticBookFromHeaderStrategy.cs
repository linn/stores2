namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class AutomaticBookFromHeaderStrategy : ICreationStrategy
    {
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IAuthorisationService authorisationService;

        private readonly IDocumentProxy documentProxy;

        public AutomaticBookFromHeaderStrategy(
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IAuthorisationService authorisationService,
            IDocumentProxy documentProxy)
        {
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.authorisationService = authorisationService;
            this.documentProxy = documentProxy;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges?.ToList();
            var authAction = AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode);
            if (!this.authorisationService.HasPermissionFor(authAction, privilegesList))
            {
                throw new UnauthorisedActionException($"You are not authorised to raise {context.Function.FunctionCode}");
            }

            if (context.IsReverseTransaction == "Y" && !this.authorisationService.HasPermissionFor(
                    AuthorisedActions.ReverseRequisition,
                    privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to reverse requisitions");
            }

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
                newPartNumber: context.NewPartNumber,
                lines: null,
                isReverseTransaction: context.IsReverseTransaction,
                originalDocumentNumber: context.OriginalReqNumber,
                bookInOrderDetails: context.BookInOrderDetails,
                dateReceived: context.DateReceived);

            var employee = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var department = await this.departmentRepository.FindByIdAsync(context.DepartmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(context.NominalCode);
            var fromLocation = await this.storageLocationRepository
                                   .FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = await this.storageLocationRepository
                                 .FindByAsync(x => x.LocationCode == context.ToLocationCode);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);

            RequisitionHeader toBeReversed = null;

            if (context.OriginalReqNumber.HasValue)
            {
                toBeReversed = await this.repository.FindByIdAsync(context.OriginalReqNumber.Value);
            }

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
                context.BatchDate,
                null,
                context.Document2Number,
                context.Document2Type,
                context.IsReverseTransaction,
                toBeReversed,
                context.DateReceived,
                context.FromCategory);


            if (context.Function.NewPartNumberRequired())
            {
                // just for the magical part number change
                var newPart = await this.partRepository.FindByIdAsync(context.NewPartNumber);
                if (newPart != null)
                {
                    req.NewPart = newPart;
                }
            }

            if (context.Function.FunctionCode == "SUKIT" || context.Function.FunctionCode == "GIST PO")
            {
                req.FromCategory = "FREE";
                req.ToCategory = "FREE";
            }

            req.Document3 = context.Document3Number;

            if (req.Document1Name == "WO" && req.Document1.HasValue)
            {
                var worksOrder = await this.documentProxy.GetWorksOrder(req.Document1.Value);
                req.WorkStationCode = worksOrder.WorkStationCode;
                req.FromCategory = req.StoresFunction.Category;

                if (req.IsReverseTransaction != "Y")
                {
                    await this.requisitionManager.AddPotentialMoveDetails(
                        req.Document1Name,
                        req.Document1.Value,
                        req.Quantity,
                        req.Part.PartNumber,
                        req.CreatedBy.Id,
                        req.ToLocation?.LocationId,
                        req.ToPalletNumber);
                }
            }

            if (req.StoresFunction.FunctionCode == "BOOKLD")
            {
                await this.requisitionManager.AddBookInOrderDetails(context.BookInOrderDetails.ToList());
            }

            await this.repository.AddAsync(req);
            
            await this.requisitionManager.CreateLinesAndBookAutoRequisitionHeader(req);

            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
