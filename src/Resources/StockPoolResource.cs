namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

    public class StockPoolResource : HypermediaResource
    {
        public string StockPoolCode { get; set; }

        public string StockPoolDescription { get; set; }

        public string DateInvalid { get; set; }

        public string AccountingCompanyCode { get; set; }

        public AccountingCompanyResource AccountingCompany { get; set; }

        public int? Sequence { get; set; }

        public string StockCategory { get; set; }

        public int? DefaultLocation { get; set; }

        public StorageLocationResource StorageLocation { get; set; }

        public int? BridgeId { get; set; }

        public string AvailableToMrp { get; set; }
    }
}
