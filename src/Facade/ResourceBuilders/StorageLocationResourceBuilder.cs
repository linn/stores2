namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Linq;
    using System.Collections.Generic;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageLocationResourceBuilder : IBuilder<StorageLocation>
    {
        public StorageLocationResource Build(StorageLocation model, IEnumerable<string> claims)
        {
            return new StorageLocationResource
            {
                LocationCode = model.LocationCode,
                Description = model.Description,
                DateInvalid = model.DateInvalid?.ToString("o"),
                DefaultStockPool = model.DefaultStockPool,
                LocationId = model.LocationId,
                LocationType = model.LocationType,
                SiteCode = model.SiteCode,
                StorageType = model.StorageTypeCode,
                StorageAreaCode = model.StorageAreaCode,
                Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(StorageLocation model)
        {
            return $"/stores2/storage/locations/{model.LocationId}";
        }

        private IEnumerable<LinkResource> BuildLinks(StorageLocation model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }

        object IBuilder<StorageLocation>.Build(StorageLocation model, IEnumerable<string> claims) => this.Build(model, claims);
    }
}
