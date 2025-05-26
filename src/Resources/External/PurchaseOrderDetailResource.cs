namespace Linn.Stores2.Resources.External
{
    using System.Collections.Generic;

    public class PurchaseOrderDetailResource
    {
        public int Line { get; set; }

        public string PartNumber { get; set; }

        public decimal OurQty { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public string RohsCompliant { get; set; }

        public IEnumerable<PurchaseOrderDeliveryResource> PurchaseDeliveries { get; set; }
    }
}
