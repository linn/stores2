namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageTypeResourceBuilder : IBuilder<StorageType>
    {
        public StorageTypeResource Build(StorageType storageType, IEnumerable<string> claims)
        {
            var storageLocationResourceBuilder = new StorageLocationResourceBuilder();

            return new StorageTypeResource
                       {
                           StorageTypeCode = storageType.StorageTypeCode,
                           Description = storageType.Description,
                           Links = this.BuildLinks(storageType, claims).ToArray()
            };
        }

        public string GetLocation(StorageType model)
        {
            return $"/stores2/storage-types/{model.StorageTypeCode}";
        }

        object IBuilder<StorageType>.Build(StorageType entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StorageType model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}