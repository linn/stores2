namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class CustRetCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authorisationService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<RequisitionHeader, int> reqRepository;

        private readonly IRequisitionManager requisitionManager;

        public CustRetCreationStrategy(
            IAuthorisationService authorisationService,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<RequisitionHeader, int> reqRepository,
            IRequisitionManager requisitionManager)
        {
            this.authorisationService = authorisationService;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.reqRepository = reqRepository;
            this.requisitionManager = requisitionManager;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            if (context.Function.FunctionCode != "CUSTRET")
            {
                throw new CreateRequisitionException("Cust Ret Creation Strategy requires a CUSTRET function");
            }

            var privilegesList = context.UserPrivileges?.ToList();
            var authAction = AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode);
            if (!this.authorisationService.HasPermissionFor(authAction, privilegesList))
            {
                throw new UnauthorisedActionException($"You are not authorised to raise CUSTRET");
            }

            if (context.Document1Type != "C" || context.Document1Number == null)
            {
                throw new CreateRequisitionException("CUSTRET function requires a credit note");
            }

            var document = this.requisitionManager.GetDocument(context.Document1Type, context.Document1Number.Value,
                context.Document1Line);

            var employee = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);
            var fromLocation = string.IsNullOrEmpty(context.FromLocationCode) ? null
                                : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = string.IsNullOrEmpty(context.ToLocationCode) ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == context.ToLocationCode);

            // create req
            var req = new RequisitionHeader(
                employee,
                context.Function,
                null,
                context.Document1Number,
                "C",
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
                part,
                context.Quantity,
                context.Document1Line,
                context.ToState,
                null,
                context.BatchRef,
                context.BatchDate,
                "FREE",
                context.Document2Number,
                context.Document2Type);


            // TODO following check from VALID_CREDIT_NOTES for 
            // requisitionManager.CheckDocument1
            // check if overbooked if line
            // check if ToState is QC,FAil there is also a document2

            // add lines and book
            await this.requisitionManager.CheckAndBookRequisitionHeader(req);

            // re-query the database for the data after stored procedures have run etc
            return await this.reqRepository.FindByIdAsync(req.ReqNumber);
        }
    }
}
