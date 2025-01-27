namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StorageArea
    {
        public string StorageAreaCode { get; set; }

        public string Description { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string SiteCode { get; set; }

        public StorageSite StorageSite { get; set; }

        public string AreaPrefix { get; set; }
    }
}
