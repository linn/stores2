namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class MoveWithLocation : ReqMove
    {
        public MoveWithLocation(StockLocator fromStockLocator, StorageLocation toLocation, decimal? quantity = null)
        {
            this.StockLocator = fromStockLocator;
            this.Quantity = quantity ?? 123m;
            this.Location = toLocation;
        }
    }
}
