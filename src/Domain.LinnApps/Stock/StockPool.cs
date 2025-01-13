namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StockPool
    {
        public int StockPoolCode { get; set; }

        public string StockPoolDescription { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string AccountingCompany { get; set; }

        public int? Sequence { get; set; }

        public string StockCategory { get; set; }

        public int? DefaultLocation { get; set; }

        public StorageLocation StorageLocation { get; set; }

        public int? BridgeId { get; set; }

        public string AvailableToMrp { get; set; }
    }
}