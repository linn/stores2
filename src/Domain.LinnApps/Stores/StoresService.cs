namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StoresService : IStoresService 
    {
        private readonly IStockService stockService;

        private readonly IRepository<StoresTransactionState, StoresTransactionStateKey> storesTransactionStateRepository;
        
        private readonly IRepository<StoresBudget, int> storesBudgetRepository;

        private readonly IRepository<StockLocator, int> stockLocatorRepository;

        private readonly IRepository<RequisitionHeader, int> requisitionRepository;

        private readonly IRepository<NominalAccount, int> nominalAccountRepository;

        private readonly IRepository<PartsStorageType, int> partStorageTypeRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        // This service is intended for stores_oo replacement methods that are not
        // suitable to be written in the requisition class itself
        public StoresService(
            IStockService stockService,
            IRepository<StoresTransactionState, StoresTransactionStateKey> storesTransactionStateRepository,
            IRepository<StoresBudget, int> storesBudgetRepository,
            IRepository<StockLocator, int> stockLocatorRepository,
            IRepository<RequisitionHeader, int> requisitionRepository,
            IRepository<NominalAccount, int> nominalAccountRepository,
            IRepository<PartsStorageType, int> partStorageTypeRepository,
            IRepository<StorageLocation, int> storageLocationRepository)
        {
            this.stockService = stockService;
            this.storesTransactionStateRepository = storesTransactionStateRepository;
            this.storesBudgetRepository = storesBudgetRepository;
            this.stockLocatorRepository = stockLocatorRepository;
            this.requisitionRepository = requisitionRepository;
            this.nominalAccountRepository = nominalAccountRepository;
            this.partStorageTypeRepository = partStorageTypeRepository;
            this.storageLocationRepository = storageLocationRepository;
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

            if (!string.IsNullOrEmpty(stateAllowedAtLocation) && stateAllowedAtLocation != "A")
            {
                if (stateAllowedAtLocation == "I" && (state.State == "QC" || state.State == "FAIL"))
                {
                    return new ProcessResult(false, "Only inspected stock can be placed on this location");
                }

                if (stateAllowedAtLocation == "Q" && state.State == "STORES")
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

            // check valid kardex move
            if (location != null && !string.IsNullOrEmpty(location.LocationCode))
            {
                var isKardexMove = Enumerable.Range(1, 5)
                    .Select(i => $"E-K{i}")
                    .Any(prefix => location.LocationCode.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
                if (isKardexMove)
                {
                    // ok to mix parts if location has no StorageType, so skip following checks if so
                    if (location.StorageType != null) 
                    {
                        var otherPartsAtLocation =
                            await this.stockLocatorRepository
                                .FilterByAsync(
                                x => 
                                    x.Quantity > 0
                                    && x.PartNumber != part.PartNumber
                                    && x.LocationId == location.LocationId);

                        // otherwise if different part(s) are at this location, cannot mix
                        if (otherPartsAtLocation.Any())
                        {
                            var msg = $"Part {otherPartsAtLocation.First().PartNumber} already at this location. "
                                      + $"Cannot mix parts since kardex location has storage type {location.StorageType.StorageTypeCode}";

                            return new ProcessResult(false, msg);
                        }
                    }
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

            if (!part.IsLive())
            {
                return new ProcessResult(false, $"Old part number {part.PartNumber} is not live");
            }

            if (newPart == null)
            {
                return new ProcessResult(false, "Part number change requires new part");
            }

            if (!newPart.IsLive())
            {
                return new ProcessResult(false, $"New part number {newPart.PartNumber} is not live");
            }

            if (part.ProductAnalysisCode != newPart.ProductAnalysisCode)
            {
                return new ProcessResult(false, $"Old part is for product group {part.ProductAnalysisCode} new part is for product group {newPart.ProductAnalysisCode}");
            }

            if (part.IsBoardPartNumber() && part.BoardNumber() != newPart.BoardNumber())
            {
                return new ProcessResult(false, $"Old part {part.PartNumber} is a different board from {newPart.PartNumber}");
            }

            // here is the line for STORES_OO_VALIDATE that says the difference is 10% but actual values are more generous
            // if not ( v_new_price / v_old_price between 0.6 and 1.7) then
            // g_error := 'Cannot price change between parts with more than 10% difference in price';
            var priceRatio = (part.BaseUnitPrice ?? 0) / (newPart.BaseUnitPrice ?? 1);
            if (priceRatio < 0.6m || priceRatio > 1.7m)
            {
                return new ProcessResult(false, $"Price change of {Math.Round(priceRatio * 100)}% not allowed");
            }

            return new ProcessResult(true, $"Part number can be changed from ${part.PartNumber} to ${newPart.PartNumber}");
        }

        public async Task<ProcessResult> ValidReverseQuantity(int originalReqNumber, decimal quantity)
        {
            // stores_oo.validate_reverse_qty
            if (quantity >= 0)
            {
                return new ProcessResult(false, "A reverse quantity must be negative");
            }

            var originalReq = await this.requisitionRepository.FindByIdAsync(originalReqNumber);
            if (originalReq == null)
            {
                return new ProcessResult(false, $"Original requisition {originalReqNumber} does not exist");
            }

            if (originalReq.Quantity < quantity * -1)
            {
                return new ProcessResult(
                    false,
                    $"Cannot reverse qty {quantity}. Original req {originalReqNumber} was for only {originalReq.Quantity}.");
            }

            return new ProcessResult(true, $"Reverse quantity of {quantity} for req {originalReqNumber} is valid");
        }

        public async Task<ProcessResult> ValidDepartmentNominal(string departmentCode, string nominalCode)
        {
            // stores_oo.valid_dept_nom
            var nominalAccount = await this.nominalAccountRepository.FindByAsync(a =>
                a.DepartmentCode == departmentCode && a.NominalCode == nominalCode);

            if (nominalAccount == null)
            {
                return new ProcessResult(false, $"Department / Nominal {departmentCode} / { nominalCode} are not a valid combination");
            }

            if (nominalAccount.StoresPostsAllowed != "Y")
            {
                return new ProcessResult(false, $"Department / Nominal {departmentCode} / {nominalCode} are not a valid for stores posting");
            }

            return new ProcessResult(true, $"Department / Nominal {departmentCode} / {nominalCode} are a valid combination for stores");
        }

        public async Task<StorageLocation> DefaultBookInLocation(string partNumber)
        {
            // stores_oo.default_bookin_location
            var storageType = await this.partStorageTypeRepository.FindByAsync(a => a.PartNumber == partNumber);

            if (storageType == null)
            {
                return null;
            }

            if (storageType.StorageTypeCode.StartsWith("K1"))
            {
                return await this.storageLocationRepository.FindByAsync(a => a.LocationCode == "E-GI-K1");
            }

            if (storageType.StorageTypeCode.StartsWith("K2"))
            {
                return await this.storageLocationRepository.FindByAsync(a => a.LocationCode == "E-GI-K2");
            }

            return null;
        }
    }
}
