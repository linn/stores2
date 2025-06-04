namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

    public class StoresPalletResource : HypermediaResource
    {
        public int PalletNumber { get; set; }

        public string Description { get; set; }

        public int StorageLocationId { get; set; }

        public StorageLocationResource StorageLocation { get; set; }

        public string DateInvalid { get; set; }

        public string DateLastAudited { get; set; }

        public string Accessible { get; set; }

        public string StoresKittable { get; set; }

        public int? StoresKittingPriority { get; set; }

        public string SalesKittable { get; set; }

        public int? SalesKittingPriority { get; set; }

        public string AllocQueueTime { get; set; }

        public LocationTypeResource LocationType { get; set; }

        public string LocationTypeId { get; set; }

        public int? AuditedBy { get; set; }

        public string DefaultStockPoolId { get; set; }

        public StockPoolResource DefaultStockPool { get; set; }

        public string StockType { get; set; }

        public string StockState { get; set; }

        public int? AuditOwnerId { get; set; }

        public int? AuditFrequencyWeeks { get; set; }

        public string AuditedByDepartmentCode { get; set; }

        public string MixStates { get; set; }

        public int? Cage { get; set; }
    }
}
