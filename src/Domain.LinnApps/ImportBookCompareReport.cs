namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class ImportBookCompareReport
    {
        public ImportBookCompareReport(
            string entryId,
            DateTime clearenceDate,
            string cosignor,
            string countryOfDispatch,
            int commodityCode,
            string cpc,
            string countryOfOrigin,
            string invoiceCurrency,
            decimal itemPrice,
            decimal customsValue,
            decimal vatValue)
        {
            this.EntryId = entryId;
            this.ClearenceDate = clearenceDate;
            this.Cosignor = cosignor;
            this.CountryOfDispatch = countryOfDispatch;
            this.CommodityCode = commodityCode;
            this.Cpc = cpc;
            this.CountryOfOrigin = countryOfOrigin;
            this.InvoiceCurrency = invoiceCurrency;
            this.ItemPrice = itemPrice;
            this.CustomsValue = customsValue;
            this.VatValue = vatValue;
        }

        public string EntryId { get; set; }

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
