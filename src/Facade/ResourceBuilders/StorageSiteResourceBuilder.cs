namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;
    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageSiteResourceBuilder : IBuilder<StorageSite>
    {
        public StorageSiteResource Build(StorageSite model, IEnumerable<string> claims)
        {
            return new StorageSiteResource
            {
                SiteCode = model.SiteCode,
                Description = model.Description,
                SitePrefix = model.SitePrefix,
                StorageAreas = model.StorageAreas.Where(a => a.DateInvalid == null).OrderBy(a => a.Description).Select(a => new StorageAreaResource
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
            throw new System.NotImplementedException();
        }

        object IBuilder<StorageSite>.Build(StorageSite model, IEnumerable<string> claims) => this.Build(model, claims);
    } 
}
