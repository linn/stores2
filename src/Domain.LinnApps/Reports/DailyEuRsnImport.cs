namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;

    public class DailyEuRsnImport
    {
        public string CountryOfOrigin { get; set; }

        public string Currency { get; set; }

        public string CustomsCpcNumber { get; set; }

        public decimal CustomsValue { get; set; }

        public string PartDescription { get; set; }

        public DateTime DocumentDate { get; set; }

        public int InvoiceNumber { get; set; }

        public string PartNumber { get; set; }

        public int? Pieces { get; set; }

        public int Quantity { get; set; }

        public string Retailer { get; set; }

        public string ReturnReason { get; set; }

        public int RsnNumber { get; set; }

        public string TariffCode { get; set; }

        public decimal? Weight { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }

        public int? SerialNumber { get; set; }

        public string GetDims()
        {
            return $"{this.Width} x {this.Height} x {this.Depth}";
        }
    }
}
