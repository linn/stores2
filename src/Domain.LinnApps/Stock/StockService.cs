namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;

    public class StockService : IStockService 
    {
        private readonly IRepository<StockLocator, int> stockLocatorRepository;

        public StockService(IRepository<StockLocator, int> stockLocatorRepository)
        {
            this.stockLocatorRepository = stockLocatorRepository;
        }

        public async Task<IList<StockLocator>> GetStockLocators(string partNumber, int? locationId, int? palletNumber)
        {
            IList<StockLocator> stockLocators;

            if (palletNumber.HasValue)
            {
                stockLocators = await this.stockLocatorRepository.FilterByAsync(
                    a => a.PalletNumber == palletNumber && a.CurrentStock == "Y" && a.PartNumber == partNumber);
            }
            else
            {
                stockLocators = await this.stockLocatorRepository.FilterByAsync(
                    a => a.LocationId == locationId && a.CurrentStock == "Y" && a.PartNumber == partNumber);
            }

            return stockLocators;
        }
    }
}
