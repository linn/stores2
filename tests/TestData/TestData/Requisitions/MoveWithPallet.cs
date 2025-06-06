namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class MoveWithPallet : ReqMove
    {
        public MoveWithPallet(StockLocator fromStockLocator, int toPalletNumber, decimal? quantity = null)
        {
            this.StockLocator = fromStockLocator;
            this.Quantity = quantity ?? 123m;
            this.PalletNumber = toPalletNumber;
        }
    }
}
