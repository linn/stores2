namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

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

        public string AccountingCompany { get; set; }

        public int? SalesAccountId { get; set; }

        public int? OutletNumber { get; set; }

        public string MixStatesFlag { get; set; }

        public string StockState { get; set; }

        public string TypeOfStock { get; set; }

        public string SpecProcFlag { get; set; }

        public string AccessibleFlag { get; set; }

        public string StoresKittableFlag { get; set; }

        public int? StoresKittingPriority { get; set; }

        public int? AuditFrequencyWeeks { get; set; }

        public string AuditedBy { get; set; }

        public string DateLastAudited { get; set; }

        public string AuditedByDepartmentCode { get; set; }

        public string AuditedByDepartmentName { get; set; }
    }
}
