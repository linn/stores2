namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    // this class really just looks up the function code
    // and then delegates the rest of the process to the relevant strategy
    public class RequisitionFactory : IRequisitionFactory
    {
        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly ICreationStrategyResolver creationStrategyResolver;

        public RequisitionFactory(
            ICreationStrategyResolver createStrategyResolver,
            IRepository<StoresFunction, string> storesFunctionRepository)
        {
            this.creationStrategyResolver = createStrategyResolver;
            this.storesFunctionRepository = storesFunctionRepository;
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
             decimal? quantity = null,
             string fromState = null,
             string toState = null)
        {
            var function = await this.storesFunctionRepository.FindByIdAsync(functionCode.ToUpper());
           
            var context = new RequisitionCreationContext
                              {
                                  Function = function,
                                  CreatedByUserNumber = createdBy,
                                  UserPrivileges = privileges,
                                  FirstLineCandidate = firstLine,
                                  ReqType = reqType,
                                  Document1Number = document1Number,
                                  Document1Type = document1Type,
                                  Document1Line = null,
                                  DepartmentCode = departmentCode,
                                  NominalCode = nominalCode,
                                  Reference = reference,
                                  Comments = comments,
                                  ManualPick = manualPick,
                                  FromStockPool = fromStockPool?.ToUpper(),
                                  ToStockPool = toStockPool?.ToUpper(),    
                                  FromLocationCode = fromLocationCode?.ToUpper(),
                                  ToLocationCode = toLocationCode?.ToUpper(),
                                  PartNumber = partNumber?.ToUpper(),
                                  Quantity = quantity,
                                  FromState = fromState?.ToUpper(),
                                  ToState = toState?.ToUpper(),
                                  FromPallet = fromPalletNumber,
                                  ToPallet = toPalletNumber
                              };

            var strategy = this.creationStrategyResolver.Resolve(context);
            var result = await strategy.Create(context);
            return result;
        }
    }
}
