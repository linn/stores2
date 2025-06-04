namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using System;
    using System.Data;

    public class StoresPallet
    {
        public StoresPallet()
        {
        }

        public StoresPallet(
            int palletNumber,
            string description,
            StorageLocation storageLocation,
            int storageLocationId,
            string accessible,
            string storesKittable,
            int? storesKittablePriority,
            string salesKittable,
            int? salesKittablePriority,
            DateTime? allocQueueTime,
            LocationType locationType,
            string locationTypeId,
            int? auditedBy,
            StockPool defaultStockPool,
            string defaultStockPoolId,
            string stockType,
            string stockState,
            int? auditOwnerId,
            int? auditFrequencyWeeks,
            string auditedByDepartmentCode,
            string mixStates,
            int? cage)
        {
            if (storageLocation == null)
            {
                throw new StoresPalletException($"Storage location {storageLocationId} not found.");
            }

            if (locationType == null && !string.IsNullOrEmpty(locationTypeId))
            {
                throw new StoresPalletException($"Location type {locationTypeId} not found.");
            }

            if (defaultStockPool == null && defaultStockPoolId != null)
            {
                throw new StoresPalletException($"Stock pool {defaultStockPoolId} not found.");
            }
            this.PalletNumber = palletNumber;
            this.Description = description;
            this.StorageLocation = storageLocation;
            this.Accessible = accessible;
            this.StoresKittable = storesKittable;
            this.StoresKittablePriority = storesKittablePriority;
            this.SalesKittable = salesKittable;
            this.SalesKittablePriority = salesKittablePriority;
            this.AllocQueueTime = allocQueueTime;
            this.LocationType = locationType;
            this.AuditedBy = auditedBy;
            this.DefaultStockPool = defaultStockPool;
            this.StockType = stockType;
            this.StockState = stockState;
            this.AuditOwnerId = auditOwnerId;
            this.AuditFrequencyWeeks = auditFrequencyWeeks;
            this.AuditedByDepartmentCode = auditedByDepartmentCode;
            this.MixStates = mixStates;
            this.Cage = cage;
        }

        public int PalletNumber { get; set; }

        public string Description { get; set; }

        public StorageLocation StorageLocation { get; set; }

        public DateTime? DateInvalid { get; set; }

        public DateTime? DateLastAudited { get; set; }

        public string Accessible { get; set; }

        public string StoresKittable { get; set; }

        public int? StoresKittablePriority { get; set; }

        public string SalesKittable { get; set; }

        public int? SalesKittablePriority { get; set; }

        public DateTime? AllocQueueTime { get; set; }

        public LocationType LocationType { get; set; }

        public int? AuditedBy { get; set; }

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
            StorageLocation storageLocation,
            int storageLocationId,
            DateTime? dateInvalid,
            DateTime? dateLastAudited,
            string accessible,
            string storesKittable,
            int? storesKittablePriority,
            string salesKittable,
            int? salesKittablePriority,
            DateTime? allocQueueTime,
            LocationType locationType,
            string locationTypeId,
            int? auditedBy,
            StockPool defaultStockPool,
            string defaultStockPoolId,
            string stockType,
            string stockState,
            int? auditOwnerId,
            int? auditFrequencyWeeks,
            string auditedByDepartmentCode,
            string mixStates,
            int? cage)
        {
            if (storageLocation == null)
            {
                throw new StoresPalletException($"Storage location {storageLocationId} not found.");
            }

            if (locationType == null && !string.IsNullOrEmpty(locationTypeId))
            {
                throw new StoresPalletException($"Location type {locationTypeId} not found.");
            }

            if (defaultStockPool == null && defaultStockPoolId != null)
            {
                throw new StoresPalletException($"Stock pool {defaultStockPoolId} not found.");
            }

            this.Description = description;
            this.StorageLocation = storageLocation;
            this.DateInvalid = dateInvalid;
            this.DateLastAudited = dateLastAudited;
            this.Accessible = accessible;
            this.StoresKittable = storesKittable;
            this.StoresKittablePriority = storesKittablePriority;
            this.SalesKittable = salesKittable;
            this.SalesKittablePriority = salesKittablePriority;
            this.AllocQueueTime = allocQueueTime;
            this.LocationType = locationType;
            this.AuditedBy = auditedBy;
            this.DefaultStockPool = defaultStockPool;
            this.StockType = stockType;
            this.StockState = stockState;
            this.AuditOwnerId = auditOwnerId;
            this.AuditFrequencyWeeks = auditFrequencyWeeks;
            this.AuditedByDepartmentCode = auditedByDepartmentCode;
            this.MixStates = mixStates;
            this.Cage = cage;
        }
    }
}
