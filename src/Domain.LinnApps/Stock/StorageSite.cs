namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System.Collections.Generic;

    public class StorageSite
    {
        public string SiteCode { get; set; }

        public string Description { get; set; }

        public string SitePrefix { get; set; }

        public ICollection<StorageArea> StorageAreas { get; set; }
    }
}
