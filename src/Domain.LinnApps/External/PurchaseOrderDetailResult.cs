namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Collections.Generic;

    public class PurchaseOrderDetailResult
    {
        public int Line { get; set; }

        public string PartNumber { get; set; }

        public decimal OurQty { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public string RohsCompliant { get; set; }

        public IEnumerable<PurchaseOrderDeliveryResult> Deliveries { get; set; }
    }
}
