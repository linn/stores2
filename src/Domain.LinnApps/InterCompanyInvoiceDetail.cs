namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Service.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;

    public class InterCompanyInvoiceDetail
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public int LineNumber { get; set; }

        public string CustomerOrderNos { get; set; }

        public string ArticleNumber { get; set; }

        public SalesArticle SalesArticle {get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal VatRate { get; set; }

        public decimal VatTotal { get; set; }

        public decimal Total { get; set; }

        public string VatCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public Country Country { get; set; }

        public int TariffId { get; set; }

        public Tariff Tariff { get; set; }

        public string LineComment { get; set; }

        public InterCompanyInvoice InterCompanyInvoice { get; set; }
    }
}
