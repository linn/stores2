namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StockPool
    {
        public int StockPoolCode { get; protected set; }

        public string StockPoolDescription { get; protected set; }

        public DateTime? DateInvalid { get; protected set; }

        public AccountingCompany AccountingCompany { get; protected set; }

        public int? Sequence { get; protected set; }

        public string StockCategory { get; protected set; }

        public int? DefaultLocation { get; protected set; }

        public StorageLocation StorageLocation { get; protected set; }

        public int? BridgeId { get; protected set; }

        public string AvailableToMrp { get; protected set; }
    }
}