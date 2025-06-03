namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Linn.Common.Domain;
    
    public interface IStockService
    {
        Task<IList<StockLocator>> GetStockLocators(string partNumber, int? locationId, int? palletNumber);
        
        Task<ProcessResult> ValidStockLocation(
            int? locationId, 
            int? palletNumber, 
            string partNumber, 
            decimal qty,
            string state,
            string stockPoolCode = null);
    }
}
