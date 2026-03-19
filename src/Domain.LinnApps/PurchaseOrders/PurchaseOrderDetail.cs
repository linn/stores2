namespace Linn.Stores2.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderDetail
    {
        public int Line { get; set; }

        public int OrderNumber { get; set; }

        public int? OurQty { get; set; }

        public string PartNumber { get; set; }

        public string RohsCompliant { get; set; }

        public SalesArticle SalesArticle { get; set; }

        public string SuppliersDesignation { get; set; }
    }
}
