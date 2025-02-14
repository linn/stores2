namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StoresService : IStoresService 
    {
        private readonly IStockService stockService;

        public StoresService(IStockService stockService)
        {
            this.stockService = stockService;
        }

        public async Task<ProcessResult> ValidOntoLocation(
            Part part,
            StorageLocation location,
            StoresPallet pallet,
            StockState state)
        {
            if (part == null || (pallet == null && location == null))
            {
                return new ProcessResult(false, "No data provided");
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
            if (state != null && stockLocators.Count > 0)
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
    }
}
