namespace Linn.Stores2.Domain.LinnApps
{
    public class DailyEuDespatchReportLine
    {
        public string RecordExporter { get; set; }

        public string RecordImporter { get; set; }

        public int CommercialInvNo { get; set; }

        public string ProductId { get; set; }

        public string HsNumber { get; set; }
        
        public string OriginCountry { get; set; }

        public int SerialNumber { get; set; }

        public int Quantity { get; set; }

        public string Currency { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CustomsTotalValue { get; set; }

        public decimal CustomPurposesValue { get; set; }

        public int QuantityPackage { get; set; }

        public decimal GrossWeight { get; set; }

        public int PackingList { get; set; }

        public string DeliveryTerms { get; set; }
    }
}
