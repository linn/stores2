﻿namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StockPool
    {
        public StockPool()
        {
        }

        public StockPool(
            string code,
            string description,
            string dateInvalid,
            AccountingCompany accountingCompany,
            int? sequence,
            string stockCategory,
            int? defaultLocation,
            StorageLocation storageLocation,
            int? bridgeId,
            string availableToMrp)
        {
            this.StockPoolCode = code.ToUpper().Trim();
            this.StockPoolDescription = description;
            this.DateInvalid = Convert.ToDateTime(dateInvalid);
            this.AccountingCompany = accountingCompany;
            this.Sequence = sequence;
            this.StockCategory = stockCategory;
            this.DefaultLocation = defaultLocation;
            this.StorageLocation = storageLocation;
            this.BridgeId = bridgeId;
            this.AvailableToMrp = availableToMrp;
        }

        public string StockPoolCode { get; set; }

        public string StockPoolDescription { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string AccountingCompanyCode { get; set; }

        public AccountingCompany AccountingCompany { get; set; }

        public int? Sequence { get; set; }

        public string StockCategory { get; set; }

        public int? DefaultLocation { get; set; }

        public StorageLocation StorageLocation { get; set; }

        public int? BridgeId { get; set; }

        public string AvailableToMrp { get; set; }

        public void Update(
            string description,
            DateTime? dateInvalid,
            AccountingCompany accountingCompany,
            int? sequence,
            string stockCategory,
            int? defaultLocation,
            StorageLocation storageLocation,
            int? bridgeId,
            string availableToMrp)
        {
            this.StockPoolDescription = description;
            this.DateInvalid = dateInvalid;
            this.AccountingCompany = accountingCompany;
            this.Sequence = sequence;
            this.StockCategory = stockCategory;
            this.DefaultLocation = defaultLocation;
            this.StorageLocation = storageLocation;
            this.BridgeId = bridgeId;
            this.AvailableToMrp = availableToMrp;
        }
    }
}