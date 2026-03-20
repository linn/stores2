namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportOrderDetailCandidate
    {
        public ImportOrderDetailCandidate()
        {
            this.Qty = 1;
        }

        public ImportOrderDetailCandidate(Rsn rsn)
        {
            this.LineType = "RSN";
            this.Rsn = rsn;
            this.Qty = rsn.Quantity;
            this.OrderDescription = rsn.SalesArticle?.Description;
            this.TariffCode = rsn.SalesArticle?.Tariff?.TariffCode;
            this.CountryOfOrigin = rsn.SalesArticle?.CountryOfOrigin;
        }

        public ImportOrderDetailCandidate(PurchaseOrder po)
        {
            this.LineType = "PO";
            this.Qty = 1; // why does everyone make this always 1?
            this.PurchaseOrder = po;
            this.OrderDescription = po.Details?.FirstOrDefault()?.SuppliersDesignation;
            this.TariffCode = po.Details?.FirstOrDefault()?.SalesArticle?.Tariff?.TariffCode;
            this.CountryOfOrigin = po.Details?.FirstOrDefault()?.SalesArticle?.CountryOfOrigin;
        }

        public int ImportBookId { get; set; }

        public string LineType { get; set; }

        public int Qty { get; set; }

        public string OrderDescription { get; set; }

        public string TariffCode { get; set; }

        public Country CountryOfOrigin { get; set; }

        public Rsn Rsn { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
