namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;

    public class DailyEuDespatchReport
    {
        public int CommercialInvNo { get; set; }

        public DateTime DateCreated { get; set; }

        public string ProductId { get; set; }

        public string ProductDescription { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public int Qty { get; set; }

        public int? QuantityPackage { get; set; }

        public int? QuantityPiecesPerPackage { get; set; }

        public string Currency { get; set; }

        public int? InvDocumentNumber { get; set; }

        public int? InvLineNo { get; set; }

        public int? LineNo { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Terms { get; set; }

        public int PackingList { get; set; }

        public decimal? NettUnitWeight { get; set; }

        public decimal? NettWeight { get; set; }

        public decimal? GrossUnitWeight { get; set; }
      
        public decimal? GrossWeight { get; set; }

        public decimal? Total { get; set; }

        public decimal? CustomsTotal { get; set; }

        public string SerialNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
    }
}
