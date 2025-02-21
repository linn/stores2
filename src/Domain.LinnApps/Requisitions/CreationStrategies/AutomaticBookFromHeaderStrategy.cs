namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
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

        public AutomaticBookFromHeaderStrategy(
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository)
        {
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
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
                context.FromState);
            await this.requisitionManager.CheckAndBookRequisitionHeader(req);

            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
