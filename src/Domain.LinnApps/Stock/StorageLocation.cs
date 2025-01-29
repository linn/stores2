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

        public StorageLocation(int locationId, string locationCode, string description, StorageSite site, StorageArea area, AccountingCompany company, string accessible, string storesKittable, string mixStates, string stockState, string typeOfStock)
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

            if (accessible == "Y" || accessible == "N")
            {
                this.AccessibleFlag = accessible;
            }
            else if (!string.IsNullOrEmpty(accessible))
            {
                throw new StorageLocationException("Cannot create Location - accessible should be Y, N or blank");
            }

            if (storesKittable == "Y" || storesKittable == "N")
            {
                this.StoresKittableFlag = storesKittable;
            }
            else if (!string.IsNullOrEmpty(storesKittable))
            {
                throw new StorageLocationException("Cannot create Location - stores kittable should be Y, N or blank");
            }

            if (mixStates == "Y" || mixStates == "N")
            {
                this.MixStatesFlag = mixStates;
            }
            else
            {
                throw new StorageLocationException("Cannot create Location - mix states should be Y or N");
            }

            if (stockState == "I" || stockState == "Q" || stockState == "A")
            {
                this.StockState = stockState;
            }
            else
            {
                throw new StorageLocationException("Cannot create Location - stock state should be I, Q or A");
            }

            if (typeOfStock == "A" || typeOfStock == "R" || typeOfStock == "F")
            {
                this.TypeOfStock = typeOfStock;
            }
            else
            {
                throw new StorageLocationException("Cannot create Location - type of stock should be R, F or A");
            }
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
    }
}
