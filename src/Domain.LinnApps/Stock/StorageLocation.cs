namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class StorageLocation
    {
        public StorageLocation()
        {
            // for ef       
        }

        public StorageLocation(
            int locationId,
            string locationCode,
            string description,
            StorageSite site,
            StorageArea area,
            AccountingCompany company,
            string accessible,
            string storesKittable,
            string mixStates,
            string stockState,
            string typeOfStock,
            StockPool stockPool,
            StorageType storageType)
        {
            this.LocationId = locationId;
            this.LocationCode = locationCode;
            this.Description = description;
            if (site == null)
            {
                throw new StorageLocationException("Location needs a site");
            }

            this.SiteCode = site.SiteCode;
            
            if (area == null)
            {
                throw new StorageLocationException("Location needs an area");
            }

            if (!string.IsNullOrEmpty(site.SitePrefix) || !string.IsNullOrEmpty(area.AreaPrefix))
            {
                var prefixShouldBe = $"{site.SitePrefix}-{area.AreaPrefix}-";
                if (!locationCode.StartsWith(prefixShouldBe))
                {
                    throw new StorageLocationException($"Cannot create Location - Location code should start with {prefixShouldBe}");
                }
            }

            this.StorageAreaCode = area.StorageAreaCode;
            this.StorageArea = area;

            if (company == null)
            {
                throw new StorageLocationException("Location needs an accounting company");
            }

            this.AccountingCompany = company.Name;

            this.CheckYesNoFlag(accessible, "Cannot create Location - accessible should be Y or N");
            this.AccessibleFlag = accessible;

            this.CheckYesNoFlag(storesKittable, "Cannot create Location - stores kittable should be Y, N or blank", true);
            this.StoresKittableFlag = storesKittable;

            this.CheckYesNoFlag(mixStates, "Cannot create Location - mix states should be Y or N");
            this.MixStatesFlag = mixStates;

            this.CheckThreeValueFlag(stockState, "I", "Q", "A", "Cannot create Location - stock state should be I, Q or A");
            this.StockState = stockState;

            this.CheckThreeValueFlag(typeOfStock, "A", "R", "F", "Cannot create Location - type of stock should be R, F or A");
            this.TypeOfStock = typeOfStock;

            this.DefaultStockPool = stockPool?.StockPoolCode;

            this.StorageTypeCode = storageType?.StorageTypeCode;
            this.StorageType = storageType;
        }

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

        public void Update(string description, AccountingCompany company, string accessible, string storesKittable,
            string mixStates, string stockState, string typeOfStock, StockPool stockPool, StorageType storageType, DateTime? dateInvalid)
        {
            this.Description = description;

            if (company == null)
            {
                throw new StorageLocationException("Location needs an accounting company");
            }

            this.AccountingCompany = company.Name;

            this.CheckYesNoFlag(accessible, "Cannot update Location - accessible should be Y or N");
            this.AccessibleFlag = accessible;

            this.CheckYesNoFlag(storesKittable, "Cannot update Location - stores kittable should be Y, N or blank", true);
            this.StoresKittableFlag = storesKittable;

            this.CheckYesNoFlag(mixStates, "Cannot update Location - mix states should be Y or N");
            this.MixStatesFlag = mixStates;

            this.CheckThreeValueFlag(stockState, "I", "Q", "A", "Cannot update Location - stock state should be I, Q or A");
            this.StockState = stockState;

            this.CheckThreeValueFlag(typeOfStock, "A", "R", "F", "Cannot update Location - type of stock should be R, F or A");
            this.TypeOfStock = typeOfStock;

            this.DefaultStockPool = stockPool?.StockPoolCode;

            this.StorageTypeCode = storageType?.StorageTypeCode;
            this.StorageType = storageType;

            this.DateInvalid = dateInvalid;
        }

        private void CheckYesNoFlag(string field, string errorMessage, bool allowNull = false)
        {
            if (string.IsNullOrEmpty(field) && allowNull)
            {
                return;
            }
            if (!(field == "Y" || field == "N"))
            {
                throw new StorageLocationException(errorMessage);
            }
        }

        private void CheckThreeValueFlag(string field, string value1, string value2, string value3, string errorMessage)
        {
            if (!(field == value1 || field == value2 || field == value3))
            {
                throw new StorageLocationException(errorMessage);
            }
        }
    }
}
