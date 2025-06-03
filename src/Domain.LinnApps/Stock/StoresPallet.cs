namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StoresPallet
    {
        public StoresPallet()
        {
        }

        public StoresPallet(
            int palletNumber,
            string description,
            StorageLocation locationId,
            DateTime? dateInvalid,
            DateTime? dateLastAudited,
            string accessible,
            string storesKittable,
            int? storesKittablePriority,
            string salesKittable,
            int? salesKittablePriority,
            DateTime? allocQueueTime,
            LocationType locationType,
            int? auditedBy,
            StockPool defaultStockPool,
            string stockType,
            string stockState,
            int? auditOwnerId,
            int? auditFrequencyWeeks,
            string auditedByDepartmentCode,
            string mixStates)
        {
            this.PalletNumber = palletNumber;
            this.Description = description;
            this.LocationIdCode = locationId.LocationId;
            this.DateInvalid = dateInvalid;
            this.DateLastAudited = dateLastAudited;
            this.Accessible = accessible;
            this.StoresKittable = storesKittable;
            this.StoresKittablePriority = storesKittablePriority;
            this.SalesKittable = salesKittable;
            this.SalesKittablePriority = salesKittablePriority;
            this.AllocQueueTime = allocQueueTime;
            this.LocationTypeId = locationType.Code;
            this.AuditedBy = auditedBy;
            this.DefaultStockPool = defaultStockPool;
            this.StockType = stockType;
            this.StockState = stockState;
            this.AuditOwnerId = auditOwnerId;
            this.AuditFrequencyWeeks = auditFrequencyWeeks;
            this.AuditedByDepartmentCode = auditedByDepartmentCode;
            this.MixStates = mixStates;
        }

        public int PalletNumber { get; set; }

        public string Description { get; set; }

        public int LocationIdCode { get; set; }

        public StorageLocation LocationId { get; set; }

        public DateTime? DateInvalid { get; set; }

        public DateTime? DateLastAudited { get; set; }

        public string Accessible { get; set; }

        public string StoresKittable { get; set; }

        public int? StoresKittablePriority { get; set; }

        public string SalesKittable { get; set; }

        public int? SalesKittablePriority { get; set; }

        public DateTime? AllocQueueTime { get; set; }

        public LocationType LocationType { get; set; }

        public string LocationTypeId { get; set; }

        public int? AuditedBy { get; set; }

        public string DefaultStockPoolId { get; set; }

        public StockPool DefaultStockPool { get; set; }

        public string StockType { get; set; }

        public string StockState { get; set; }

        public int? AuditOwnerId { get; set; }

        public int? AuditFrequencyWeeks { get; set; }

        public string AuditedByDepartmentCode { get; set; }

        public string MixStates { get; set; }

        public int? Cage { get; set; }

        public void Update(
            string description,
            StorageLocation locationId,
            DateTime? dateInvalid,
            DateTime? dateLastAudited,
            string accessible,
            string storesKittable,
            int? storesKittablePriority,
            string salesKittable,
            int? salesKittablePriority,
            DateTime? allocQueueTime,
            LocationType locationType,
            int? auditedBy,
            StockPool defaultStockPool,
            string stockType,
            string stockState,
            int? auditOwnerId,
            int? auditFrequencyWeeks,
            string auditedByDepartmentCode,
            string mixStates)
        {
            this.Description = description;
            this.LocationIdCode = locationId.LocationId;
            this.DateInvalid = dateInvalid;
            this.DateLastAudited = dateLastAudited;
            this.Accessible = accessible;
            this.StoresKittable = storesKittable;
            this.StoresKittablePriority = storesKittablePriority;
            this.SalesKittable = salesKittable;
            this.SalesKittablePriority = salesKittablePriority;
            this.AllocQueueTime = allocQueueTime;
            this.LocationTypeId = locationType.Code;
            this.AuditedBy = auditedBy;
            this.DefaultStockPool = defaultStockPool;
            this.StockType = stockType;
            this.StockState = stockState;
            this.AuditOwnerId = auditOwnerId;
            this.AuditFrequencyWeeks = auditFrequencyWeeks;
            this.AuditedByDepartmentCode = auditedByDepartmentCode;
            this.MixStates = mixStates;
        }
    }
}
