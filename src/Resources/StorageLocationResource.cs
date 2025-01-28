using Linn.Common.Resources;

namespace Linn.Stores2.Resources
{
    public class StorageLocationResource : HypermediaResource
    {
        public int LocationId { get; set; }

        public string LocationCode { get; set; }

        public string Description { get; set; }

        public string LocationType { get; set; }

        public string DefaultStockPool { get; set; }

        public string DateInvalid { get; set; }

        public string StorageType { get; set; }

        public string StorageTypeDescription { get; set; }

        public string SiteCode { get; set; }

        public string StorageAreaCode { get; set; }
    }
}
