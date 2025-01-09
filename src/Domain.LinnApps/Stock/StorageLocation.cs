namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StorageLocation
    {
        public int LocationId { get; set; }

        public string LocationCode { get; set; }

        public string Description { get; set; }

        public string LocationType { get; set; }

        public string DefaultStockPool { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string StorageType { get; set; }

        public string SiteCode { get; set; }
    }
}
