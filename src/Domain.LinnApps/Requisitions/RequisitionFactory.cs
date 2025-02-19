namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionFactory : IRequisitionFactory
    {
        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly ICreationStrategyResolver creationStrategyResolver;

        public RequisitionFactory(
            ICreationStrategyResolver createStrategyResolver,
            IRepository<StoresFunction, string> storesFunctionRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository)
        {
            this.creationStrategyResolver = createStrategyResolver;
            this.storesFunctionRepository = storesFunctionRepository;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
        }

        public async Task<RequisitionHeader> CreateRequisition(
             int createdBy,
             IEnumerable<string> privileges,
             string functionCode,
             string reqType,
             int? document1Number,
             string document1Type,
             string departmentCode,
             string nominalCode,
             LineCandidate firstLine = null,
             string reference = null,
             string comments = null,
             string manualPick = null,
             string fromStockPool = null,
             string toStockPool = null,
             int? fromPalletNumber = null,
             int? toPalletNumber = null,
             string fromLocationCode = null,
             string toLocationCode = null,
             string partNumber = null,
             decimal? qty = null)
        {
            var who = await this.employeeRepository.FindByIdAsync(createdBy);
            var function = await this.storesFunctionRepository.FindByIdAsync(functionCode);
            var department = await this.departmentRepository.FindByIdAsync(departmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(nominalCode);
            var part = await this.partRepository.FindByIdAsync(partNumber);

            var fromLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == fromLocationCode);

            var toLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == toLocationCode);
            
            // pass stuff common to all function codes, do basic validation in the constructor
            // todo - maybe some of this is still function specific and could move into strategies
            var req = new RequisitionHeader(
                who,
                function,
                reqType,
                document1Number,
                document1Type,
                department,
                nominal,
                reference: reference,
                comments: comments,
                manualPick: manualPick,
                fromStockPool: fromStockPool,
                toStockPool: toStockPool,
                fromPalletNumber: fromPalletNumber,
                toPalletNumber: toPalletNumber,
                fromLocation: fromLocation,
                toLocation: toLocation,
                part: part,
                qty: qty);

            // todo - move below into its own strategy
            // if (!string.IsNullOrEmpty(partNumber) && function.FunctionType == "A")
            // {
            //     // function automatically booked from the header info
            //     await this..CheckAndBookRequisitionHeader(req);
            // }
            // else
            // {
            //     await this.AddRequisitionLine(req, firstLine);
            // }


            // now resolve the correct strategy for the function code at hand
            var strategy = this.creationStrategyResolver.Resolve(functionCode);

            // and apply it
            await strategy.Apply(req, firstLine, who.Id, privileges);

            // return the newly created and validated req
            // assuming the strategy has done all the necessary business
            return req;
        }
    }
}
