using Linn.Stores2.Domain.LinnApps.Requisitions;
using System.Collections.Generic;

namespace Linn.Stores2.Domain.LinnApps.Stock
{
    public class StorageSite
    {
        public string SiteCode { get; set; }

        public string Description { get; set; }

        public string SitePrefix { get; set; }

        public ICollection<StorageArea> StorageAreas { get; set; }
    }
}
