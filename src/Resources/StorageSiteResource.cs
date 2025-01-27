﻿namespace Linn.Stores2.Resources
{
    using System.Collections.Generic;

    public class StorageSiteResource
    {
        public string SiteCode { get; set; }

        public string Description { get; set; }

        public string SitePrefix { get; set; }

        public IEnumerable<StorageAreaResource> StorageAreas { get; set; }
    }
}
