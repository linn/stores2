using Linn.Stores2.Resources.Requisitions;
using System.Collections.Generic;

namespace Linn.Stores2.Resources
{
    public class StorageSiteResource
    {
        public string SiteCode { get; set; }

        public string Description { get; set; }

        public string SitePrefix { get; set; }

        public IEnumerable<StorageAreaResource> StorageAreas { get; set; }
    }
}
