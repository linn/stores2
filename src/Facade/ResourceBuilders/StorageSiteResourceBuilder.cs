namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageSiteResourceBuilder : IBuilder<StorageSite>
    {
        public StorageSiteResource Build(StorageSite model, IEnumerable<string> claims)
        {
            return new StorageSiteResource
            {
                Links = new LinkResource[1] { new LinkResource("self", this.GetLocation(model)) },
                SiteCode = model.Code,
                Description = model.Description,
                SitePrefix = model.Prefix,
                StorageAreas = model.StorageAreas
                    ?.Where(a => a.DateInvalid == null)
                    .OrderBy(a => a.Description)
                    .Select(a => new StorageAreaResource 
                                     {
                                         StorageAreaCode = a.StorageAreaCode,
                                         Description = a.Description,
                                         AreaPrefix = a.AreaPrefix,
                                         DateInvalid = a.DateInvalid?.ToString("o")
                                     })
            };
        }

        public string GetLocation(StorageSite model)
        {
            return $"/stores2/storage/sites/{model.Code}";
        }

        object IBuilder<StorageSite>.Build(StorageSite model, IEnumerable<string> claims) => this.Build(model, claims);
    } 
}
