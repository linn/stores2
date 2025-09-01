namespace Linn.Stores2.Domain.LinnApps
{
    public class DailyEuImportRsnReportLine
    {
        public int IntercompanyInvoice { get; set; }

        public int Pieces { get; set; }

        public int Weight { get; set; }

        public string Dims { get; set; }

        public string RetailerDetails { get; set; }

        public int RsnNumber { get; set; }

        public string PartNumber { get; set; }

        public int SerialNumber { get; set; }

        public string Description { get; set; }

        public string ReturnReason { get; set; }

        public string CustomsCpcNumber { get; set; }

        public string TarrifCode { get; set; }

        public string OriginCountry { get; set; }

        public int Qty { get; set; }

        public string Currency { get; set; }

        public decimal CustomsValue { get; set; }
    }
}