namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StockPoolResourceBuilder : IBuilder<StockPool>
    {
        private readonly IBuilder<StorageLocation> storageLocationResourceBuilder;

        public StockPoolResourceBuilder(IBuilder<StorageLocation> storageLocationResourceBuilder)
        {
            this.storageLocationResourceBuilder = storageLocationResourceBuilder;
        }

        public StockPoolResource Build(StockPool stockPool, IEnumerable<string> claims)
        {
            if (stockPool == null)
            {
                return null;
            }

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
                         DefaultLocationName = stockPool.StorageLocation?.LocationCode,
                         StorageLocation = stockPool.StorageLocation == null ? null : (StorageLocationResource)this.storageLocationResourceBuilder.Build(stockPool.StorageLocation, claims),
                         BridgeId = stockPool.BridgeId,
                         AvailableToMrp = stockPool.AvailableToMrp,
                         Links = this.BuildLinks(stockPool, claims).ToArray()
            };
        }

        public string GetLocation(StockPool model)
        {
            return $"/stores2/stock-pools/{model.StockPoolCode}";
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
