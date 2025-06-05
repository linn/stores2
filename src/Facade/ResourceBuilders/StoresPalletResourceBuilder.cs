namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StoresPalletResourceBuilder : IBuilder<StoresPallet>
    {
        private readonly StorageLocationResourceBuilder storageLocationResourceBuilder;
        private readonly LocationTypeResourceBuilder locationTypeResourceBuilder;
        private readonly StockPoolResourceBuilder stockPoolResourceBuilder;

        public StoresPalletResourceBuilder(
            StorageLocationResourceBuilder storageLocationResourceBuilder,
            LocationTypeResourceBuilder locationTypeResourceBuilder,
            StockPoolResourceBuilder stockPoolResourceBuilder)
        {
            this.storageLocationResourceBuilder = storageLocationResourceBuilder;
            this.locationTypeResourceBuilder = locationTypeResourceBuilder;
            this.stockPoolResourceBuilder = stockPoolResourceBuilder;
        }

        public StoresPalletResource Build(StoresPallet pallet, IEnumerable<string> claims)
        {
            var storageLocation = this.storageLocationResourceBuilder.Build(pallet.StorageLocation, claims);

            var locationType = pallet.LocationType != null
                                   ? this.locationTypeResourceBuilder.Build(pallet.LocationType, claims)
                                   : null;

            var stockPool = pallet.DefaultStockPool != null
                                ? this.stockPoolResourceBuilder.Build(pallet.DefaultStockPool, claims)
                                : null;

            return new StoresPalletResource
            {
                PalletNumber = pallet.PalletNumber,
                Description = pallet.Description,
                StorageLocationId = pallet.StorageLocation.LocationId,
                StorageLocation = storageLocation,
                DateInvalid = pallet.DateInvalid?.ToString("o"),
                DateLastAudited = pallet.DateLastAudited?.ToString("o"),
                Accessible = pallet.Accessible,
                StoresKittable = pallet.StoresKittable,
                StoresKittingPriority = pallet.StoresKittingPriority,
                SalesKittable = pallet.SalesKittable,
                SalesKittingPriority = pallet.SalesKittingPriority,
                AllocQueueTime = pallet.AllocQueueTime?.ToString("o"),
                LocationType = locationType,
                LocationTypeId = pallet.LocationType?.Code,
                AuditedBy = pallet.AuditedBy,
                DefaultStockPoolId = pallet.DefaultStockPool?.StockPoolCode,
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
