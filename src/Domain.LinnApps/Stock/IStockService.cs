namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStockService
    {
        Task<IList<StockLocator>> GetStockLocators(string partNumber, int? locationId, int? palletNumber);
    }
}
