namespace Linn.Stores2.Domain.LinnApps.Stock
{
    public class StoragePlace
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? LocationId { get; set; }

        public string LocationCode { get; set; }

        public int? PalletNumber { get; set; }

        public string SiteCode { get; set; }
    }
}
