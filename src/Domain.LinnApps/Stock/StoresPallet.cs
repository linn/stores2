﻿namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

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
            int? storesKittingPriority,
            string salesKittable,
            int? salesKittingPriority,
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
            string cage)
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
            this.StoresKittingPriority = storesKittingPriority;
            this.SalesKittable = salesKittable;
            this.SalesKittingPriority = salesKittingPriority;
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

        public int? StoresKittingPriority { get; set; }

        public string SalesKittable { get; set; }

        public int? SalesKittingPriority { get; set; }

        public DateTime? AllocQueueTime { get; set; }

        public LocationType LocationType { get; set; }

        public int? AuditedBy { get; set; }

        public Employee AuditedByEmployee { get; set; }

        public StockPool DefaultStockPool { get; set; }

        public string StockType { get; set; }

        public string StockState { get; set; }

        public int? AuditOwnerId { get; set; }

        public int? AuditFrequencyWeeks { get; set; }

        public string AuditedByDepartmentCode { get; set; }

        public Department AuditedByDepartment { get; set; }

        public string MixStates { get; set; }

        public string Cage { get; set; }

        public void Update(
            string description,
            StorageLocation storageLocation,
            int storageLocationId,
            DateTime? dateInvalid,
            DateTime? dateLastAudited,
            string accessible,
            string storesKittable,
            int? storesKittingPriority,
            string salesKittable,
            int? salesKittingPriority,
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
            string cage)
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
            this.StoresKittingPriority = storesKittingPriority;
            this.SalesKittable = salesKittable;
            this.SalesKittingPriority = salesKittingPriority;
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
