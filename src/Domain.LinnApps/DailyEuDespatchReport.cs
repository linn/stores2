namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class DailyEuDespatchReport
    {
        public string AccountName { get; set; }

        public string Country { get; set; }

        public DateTime DateCreated { get; set; }

        public int ExbookId { get; set; }

        public int LineNo { get; set; }

        public int InvDocumentNumber { get; set; }

        public int InvLineNo { get; set; }

        public string Currency { get; set; }

        public string ArticleNumber { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public int Qty { get; set; }

        public int UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string CustomsTotal { get; set; }

        public decimal UnitWeight { get; set; }

        public string Terms { get; set; }

        public int ConsignmentId { get; set; }

        public decimal NetWeight { get; set; }

        public string ConsignmentItemType { get; set; }

        public decimal QuantityPackage { get; set; }

        public decimal QuantityPiecesPerPackage { get; set; }
    }
}
