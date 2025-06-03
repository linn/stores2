namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StoresPalletResourceBuilder : IBuilder<StoresPallet>
    {
        public StoresPalletResource Build(StoresPallet pallet, IEnumerable<string> claims)
        {
            var locationid = new StorageLocationResourceBuilder().Build(pallet.LocationId, claims);

            var locationType = new LocationTypeResourceBuilder().Build(pallet.LocationType, claims);

            var stockPool = new StockPoolResourceBuilder().Build(pallet.DefaultStockPool, claims);

            return new StoresPalletResource
            {
                PalletNumber = pallet.PalletNumber,
                Description = pallet.Description,
                LocationIdCode = pallet.LocationIdCode,
                LocationId = locationid,
                DateInvalid = pallet.DateInvalid?.ToString("o"),
                DateLastAudited = pallet.DateLastAudited?.ToString("o"),
                Accessible = pallet.Accessible,
                StoresKittable = pallet.StoresKittable,
                StoresKittablePriority = pallet.StoresKittablePriority,
                SalesKittable = pallet.SalesKittable,
                SalesKittablePriority = pallet.SalesKittablePriority,
                AllocQueueTime = pallet.AllocQueueTime?.ToString("o"),
                LocationType = locationType,
                LocationTypeId = pallet.LocationTypeId,
                AuditedBy = pallet.AuditedBy,
                DefaultStockPoolId = pallet.DefaultStockPoolId,
                DefaultStockPool = stockPool,
                StockType = pallet.StockType,
                StockState = pallet.StockState,
                AuditOwnerId = pallet.AuditOwnerId,
                AuditFrequencyWeeks = pallet.AuditFrequencyWeeks,
                AuditedByDepartmentCode = pallet.AuditedByDepartmentCode,
                MixStates = pallet.MixStates,
                Cage = pallet.Cage,
                Links = this.BuildLinks(pallet, claims).ToArray()
            };
        }

        public string GetLocation(StoresPallet model)
        {
            return $"/stores2/pallets/{model.PalletNumber}";
        }

        object IBuilder<StoresPallet>.Build(StoresPallet entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StoresPallet model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
