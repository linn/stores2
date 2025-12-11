namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class DailyEuDespatchReport
    {
        public int CommercialInvNo { get; set; }

        public DateTime DateCreated { get; set; }

        public string ProductId { get; set; }

        public DateTime DocumentDate { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public int Qty { get; set; }

        public int QuantityPackage { get; set; }

        public int QuantityPiecesPerPackage { get; set; }

        public string Currency { get; set; }

        public int InvDocumentNumber { get; set; }

        public int InvLineNo { get; set; }

        public int LineNo { get; set; }

        public int UnitPrice { get; set; }

        public string Terms { get; set; }

        public int PackingList { get; set; }

        public decimal Weight { get; set; }

        public decimal CustomsTotal { get; set; }
    }
}
