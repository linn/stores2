namespace Linn.Stores2.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class StorageSiteResource : HypermediaResource
    {
        public string SiteCode { get; set; }

        public string Description { get; set; }

        public string SitePrefix { get; set; }

        public IEnumerable<StorageAreaResource> StorageAreas { get; set; }
    }
}
