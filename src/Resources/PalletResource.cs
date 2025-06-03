namespace Linn.Stores2.Resources
{
    using System;
    using System.Reflection;

    using Linn.Common.Resources;

    public class PalletResource : HypermediaResource
    {
        public int PalletNumber { get; set; }

        public string Description { get; set; }

        public int LocationIdCode { get; set; }

        public StorageLocationResource LocationId { get; set; }

        public string DateInvalid { get; set; }

        public string DateLastAudited { get; set; }

        public string Accessible { get; set; }

        public string StoresKittable { get; set; }

        public int? StoresKittablePriority { get; set; }

        public string SalesKittable { get; set; }

        public int? SalesKittablePriority { get; set; }

        public string AllocQueueTime { get; set; }

        public string Queue { get; set; }

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
