namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class ImportBookCompareReport
    {
        public int EntryId { get; set; }

        public DateTime ClearenceDate { get; set; }

        public string Cosignor { get; set; }

        public string CountryOfDispatch { get; set; }

        public int CommodityCode { get; set; }

        public string Cpc { get; set; }

        public string CountryOfOrigin { get; set; }

        public string InvoiceCurrency { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal CustomsValue { get; set; }

        public decimal VatValue { get; set; }
    }
}
