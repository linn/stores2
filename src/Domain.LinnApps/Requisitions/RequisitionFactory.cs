namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
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
            int? document1Line,
            string document1Type,
            int? document2Number,
            string document2Type,
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
            string newPartNumber = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null,
            IEnumerable<LineCandidate> lines = null,
            string isReverseTransaction = "N",
            int? originalReqNumber = null,
            int? document3Number = null)
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
                                  Document1Line = document1Line,
                                  Document2Number = document2Number,
                                  Document2Type = document2Type,
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
                                  NewPartNumber = newPartNumber?.ToUpper(),
                                  Quantity = quantity,
                                  FromState = fromState?.ToUpper(),
                                  ToState = toState?.ToUpper(),
                                  FromPallet = fromPalletNumber,
                                  ToPallet = toPalletNumber,
                                  BatchRef = batchRef,
                                  BatchDate = batchDate,
                                  Lines = lines,
                                  IsReverseTransaction = isReverseTransaction,
                                  OriginalReqNumber = originalReqNumber,
                                  Document3Number = document3Number
                              };

            var strategy = this.creationStrategyResolver.Resolve(context);
            var result = await strategy.Create(context);
            return result;
        }
    }
}
