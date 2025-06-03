namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class PalletResourceBuilder : IBuilder<Pallet>
    {
        public PalletResource Build(Pallet pallet, IEnumerable<string> claims)
        {
            return new PalletResource
            {
                PalletNumber = pallet.PalletNumber,
                Description = pallet.Description,
                LocationIdCode = pallet.LocationIdCode,
                LocationId = new StorageLocationResourceBuilder().Build(pallet.LocationId, claims),
                DateInvalid = pallet.DateInvalid?.ToString("o"),
                DateLastAudited = pallet.DateLastAudited?.ToString("o"),
                Accessible = pallet.Accessible,
                StoresKittable = pallet.StoresKittable,
                StoresKittablePriority = pallet.StoresKittablePriority,
                SalesKittable = pallet.SalesKittable,
                SalesKittablePriority = pallet.SalesKittablePriority,
                AllocQueueTime = pallet.AllocQueueTime?.ToString("o"),
                LocationType = new LocationTypeResourceBuilder().Build(pallet.LocationType, claims),
                LocationTypeId = pallet.LocationTypeId,
                AuditedBy = pallet.AuditedBy,
                DefaultStockPoolId = pallet.DefaultStockPoolId,
                DefaultStockPool = new StockPoolResourceBuilder().Build(pallet.DefaultStockPool, claims),
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

        public string GetLocation(Pallet model)
        {
            return $"/stores2/stock-pool/{model.PalletNumber}";
        }

        object IBuilder<Pallet>.Build(Pallet entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Pallet model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
