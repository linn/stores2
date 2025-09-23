namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class DailyEuRsnImportReport
    {
        public string RsnNumber { get; set; }

        public string Addresse { get; set; }

        public string Country { get; set; }

        public DateTime DocumentDate { get; set; }

        public int InvoiceNumber { get; set; }

        public string PartNo { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public int Qty { get; set; }

        public int QuantityPackage { get; set; }

        public int QuantityPiecesPerPackage { get; set; }

        public string Currency { get; set; }

        public int LineUnitPrice { get; set; }

        public int LineTotal { get; set; }

        public int UnitPrice { get; set; }

        public int Total { get; set; }

        public string CustomsTotal { get; set; }

        public int NetWeightKg { get; set; }

        public int GrossWeightKg { get; set; }

        public int PackingList { get; set; }

        public string Terms { get; set; }

        public int InvoiceLine { get; set; }

        public string Upgrade { get; set; }
    }
}
