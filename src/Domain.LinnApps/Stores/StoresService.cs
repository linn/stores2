namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Org.BouncyCastle.Ocsp;

    public class StoresService : IStoresService 
    {
        private readonly IStockService stockService;

        private readonly IRepository<StoresTransactionState, StoresTransactionStateKey> storesTransactionStateRepository;
        
        private readonly IRepository<StoresBudget, int> storesBudgetRepository;

        // This service is intended for stores_oo replacement methods that are not
        // suitable to be written in the requisition class itself
        public StoresService(
            IStockService stockService,
            IRepository<StoresTransactionState, StoresTransactionStateKey> storesTransactionStateRepository,
            IRepository<StoresBudget, int> storesBudgetRepository)
        {
            this.stockService = stockService;
            this.storesTransactionStateRepository = storesTransactionStateRepository;
            this.storesBudgetRepository = storesBudgetRepository;
        }

        public async Task<ProcessResult> ValidOntoLocation(
            Part part,
            StorageLocation location,
            StoresPallet pallet,
            StockState state)
        {
            if (part == null || state == null || (pallet == null && location == null))
            {
                var errorMessage = part == null ? "No valid part provided" : "No valid onto location or state provided";
                
                return new ProcessResult(false, errorMessage);
            }

            var stockLocators = await this.stockService.GetStockLocators(
                part.PartNumber,
                location?.LocationId,
                pallet?.PalletNumber);

            var typeOfStock = location?.TypeOfStock ?? pallet?.TypeOfStock;
            var stateAllowedAtLocation = location?.StockState ?? pallet?.StockState;
            
            if (!string.IsNullOrEmpty(typeOfStock) && typeOfStock != "A")
            {
                if (part.RawOrFinished != typeOfStock)
                {
                    return new ProcessResult(
                        false,
                        $"Location/Pallet is for {typeOfStock} but part {part.PartNumber} is {part.RawOrFinished}");
                }
            }

            if (!string.IsNullOrEmpty(stateAllowedAtLocation) && state != null && stateAllowedAtLocation != "A")
            {
                if (stateAllowedAtLocation == "I" && (state.State == "QC" || state.State == "FAIL"))
                {
                    return new ProcessResult(false, "Only inspected stock can be placed on this location");
                }
                else if (stateAllowedAtLocation == "Q" && state.State == "STORES")
                {
                    return new ProcessResult(false, "Only uninspected/failed stock can be placed on this location");
                }
            }

            // mix states currently not allowed ever
            if (stockLocators.Count > 0)
            {
                if (stockLocators.Any(a => a.State != state.State))
                {
                    return new ProcessResult(
                        false,
                        $"Stock not in state {state.State} already exists at this location");
                }
            }

            return new ProcessResult(true, $"Part {part.PartNumber} is valid for this location");
        }

        public async Task<ProcessResult> ValidState(
            string transactionCode,
            StoresFunction storesFunction,
            string stateCode,
            string fromOrOnto)
        {
            if (!string.IsNullOrEmpty(transactionCode))
            {
                var storesTransactionState = await this.storesTransactionStateRepository.FindByAsync(
                    a => a.State == stateCode && a.FromOrOnto == fromOrOnto && a.TransactionCode == transactionCode);

                if (storesTransactionState == null)
                {
                    return new ProcessResult(
                        false,
                        $"State {stateCode} is not valid for {fromOrOnto} for {transactionCode}");
                }
            }

            if (storesFunction != null)
            {
                foreach (var storesFunctionTransactionsType in storesFunction.TransactionsTypes)
                {
                    var storesTransactionState = await this.storesTransactionStateRepository
                                                     .FindByAsync(
                                                         a => a.State == stateCode
                                                              && a.FromOrOnto == fromOrOnto
                                                              && a.TransactionCode == storesFunctionTransactionsType.TransactionCode);
                    if (storesTransactionState != null)
                    {
                        return new ProcessResult(
                            true,
                            $"State {stateCode} is valid for {fromOrOnto} for {storesFunctionTransactionsType.TransactionCode}");
                    }
                }

                return new ProcessResult(
                    false,
                    $"State {stateCode} is not valid for {fromOrOnto} for {storesFunction.FunctionCode}");
            }

            return new ProcessResult(true, "State is valid");
        }

        public ProcessResult ValidStockPool(Part part, StockPool stockPool)
        {
            if (part == null || stockPool == null)
            {
                return new ProcessResult(false, "Incomplete stock pool or part data supplied");
            }

            if (stockPool.AccountingCompanyCode != part.AccountingCompanyCode)
            {
                return new ProcessResult(
                    false,
                    $"Stock Pool {stockPool.StockPoolCode} is for {stockPool.AccountingCompanyCode} and is not valid for part {part.PartNumber}");
            }

            return new ProcessResult(true, $"Stock Pool {stockPool.StockPoolCode} is valid for part {part.PartNumber}");
        }

        public async Task<ProcessResult> ValidPoQcBatch(string batchRef, int orderNumber, int orderLine)
        {
            if (batchRef.Substring(1) != orderNumber.ToString())
            {
                return new ProcessResult(
                    false, $"You are trying to pass batch {batchRef} for payment against the wrong Order Number: {orderNumber}");
            }

            var storesBudgets = await this.storesBudgetRepository.FilterByAsync(
                x => x.RequisitionLine.Document1Number == orderNumber
                     && x.RequisitionLine.Document1Line == orderLine);
            
            var bookedIn = storesBudgets.Where(x => x.TransactionCode == "SUGII").ToList();

            if (bookedIn.Count == 0 || bookedIn.Sum(x => x.Quantity) == 0)
            {
                return new ProcessResult(
                    false, $"Nothing found to pass for payment - Check order {orderNumber} has been booked in.");
            }

            var passedForPayment = storesBudgets.Where(x => x.TransactionCode == "GISTI1");

            if (bookedIn.Sum(x => 
                    x.RequisitionLine.RequisitionHeader.IsReverseTransaction == "Y" ? -x.Quantity : x.Quantity) 
                == passedForPayment.Sum(x =>
                    x.RequisitionLine.RequisitionHeader.IsReverseTransaction == "Y" ? -x.Quantity : x.Quantity))
            {
                return new ProcessResult(
                    false, $"Everything on {batchRef} has already been passed for payment");
            }
            
            return new ProcessResult(true, $"Order {orderNumber} is valid for part {orderLine}");
        }

        public ProcessResult ValidPartNumberChange(Part part, Part newPart)
        {
            // logic from STORES_OO_VALIDATE.VALID_PART_NUMBER_CHANGE

            if (part == null)
            {
                return new ProcessResult(false, "Part number change requires old part");
            }

            if (newPart == null)
            {
                return new ProcessResult(false, "Part number change requires new part");
            }

            if (part.ProductAnalysisCode != newPart.ProductAnalysisCode)
            {
                return new ProcessResult(false, $"Old part is for product group {part.ProductAnalysisCode} new part is for product group {newPart.ProductAnalysisCode}");
            }

            return new ProcessResult(true, $"Part number can be changed from ${part.PartNumber} to ${newPart.PartNumber}");
        }
    }
}
