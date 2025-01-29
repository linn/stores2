using Linn.Stores2.Domain.LinnApps.Accounts;

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

        public string StorageTypeCode { get; set; }

        public StorageType StorageType { get; set; }

        public string SiteCode { get; set; }

        public string StorageAreaCode { get; set; }

        public StorageArea StorageArea { get; set; }

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

        public int? AuditedByEmployeeId { get; set; }

        public Employee AuditedBy { get; set; }

        public DateTime? DateLastAudited { get; set; }

        public string AuditedByDepartmentCode { get; set; }

        public Department AuditedByDepartment { get; set; }

        public bool MixStates() => MixStatesFlag == "Y";

        public bool Accessible() => AccessibleFlag == "Y";

        public bool StoresKittable() => StoresKittableFlag == "Y";
    }
}
