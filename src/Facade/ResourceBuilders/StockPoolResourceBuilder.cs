namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StockPoolResourceBuilder : IBuilder<StockPool>
    {
        public StockPoolResource Build(StockPool stockPool, IEnumerable<string> claims)
        {
            return new StockPoolResource
            {
                         StockPoolCode  = stockPool.StockPoolCode,
                         StockPoolDescription = stockPool.StockPoolDescription,
                         DateInvalid = stockPool.DateInvalid?.ToString("o"),
                         AccountingCompanyCode = stockPool.AccountingCompanyCode, 
                         AccountingCompany = stockPool.AccountingCompany == null ? null : new AccountingCompanyResource
                                                 {
                                                     Id = stockPool.AccountingCompany.Id,
                                                     Name = stockPool.AccountingCompany.Name,
                                                     Description = stockPool.AccountingCompany.Description,
                                                     Sequence = stockPool.AccountingCompany.Sequence
                                                 },
                         Sequence = stockPool.Sequence,
                         StockCategory = stockPool.StockCategory,
                         DefaultLocation = stockPool.DefaultLocation,
                         StorageLocation = stockPool.StorageLocation == null? null : new StorageLocationResource
                                               {
                                                   LocationCode = stockPool.StorageLocation.LocationCode,
                                                   Description = stockPool.StorageLocation.Description,
                                                   DateInvalid = stockPool.StorageLocation.DateInvalid?.ToString("o"),
                                                   DefaultStockPool = stockPool.StorageLocation.DefaultStockPool,
                                                   LocationId = stockPool.StorageLocation.LocationId,
                                                   LocationType = stockPool.StorageLocation.LocationType,
                                                   SiteCode = stockPool.StorageLocation.SiteCode,
                                                   StorageType = stockPool.StorageLocation.StorageType
                         },
                         BridgeId = stockPool.BridgeId,
                         AvailableToMrp = stockPool.AvailableToMrp
            };
        }

        public string GetLocation(StockPool model)
        {
            return $"/stores2/stock-pool/{model.StockPoolCode}";
        }

        object IBuilder<StockPool>.Build(StockPool entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StockPool model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
