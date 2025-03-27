using System.Linq;
using Linn.Common.Domain;

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

        public async Task<ProcessResult> ValidStockLocation(
            int? locationId, 
            int? palletNumber, 
            string partNumber, 
            decimal qty, 
            string state)
        {
            var stockData = await this.stockLocatorRepository
                .FilterByAsync(x => x.PartNumber == partNumber 
                                    && (!locationId.HasValue || x.LocationId == locationId)
                                    && (!palletNumber.HasValue || x.PalletNumber == palletNumber));

            var groups = stockData.GroupBy(s => s.State)
                .Select(g => new
                {
                    State = g.Key,
                    QtyFree = g.Sum(s => s.Quantity - (s.QuantityAllocated ?? 0))
                })
                .Where(s => s.QtyFree > 0)
                .ToList();

            if (groups.Count == 0)
            {
                return new ProcessResult { Success = false, Message = $"Part {partNumber} not at this location" };
            }

            var matchesState = false;
            var stateQty = 0m;

            foreach (var stock in groups.Where(stock => !string.IsNullOrEmpty(state) && stock.State == state))
            {
                matchesState = true;
                stateQty = stock.QtyFree.GetValueOrDefault();
            }

            if (!matchesState && !string.IsNullOrEmpty(state))
            {
                return new ProcessResult { Success = false, Message = $"No stock at this location with state {state}" };
            }

            if (string.IsNullOrEmpty(state) && groups.Skip(1).Any())
            {
                return new ProcessResult { Success = false, Message = "More than one state at this location for part, need to enter one" };
            }

            if (!string.IsNullOrEmpty(state) && stateQty < qty)
            {
                return new ProcessResult
                {
                    Success = false, 
                    Message = $"Not enough stock at this location, unallocated qty: {stateQty}"
                };
            }

            return new ProcessResult { Success = true };
        }
    }
}
