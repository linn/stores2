namespace Linn.Stores2.Resources.External
{
    public class PurchaseOrderDeliveryResource
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public decimal OurQty { get; set; }

        public decimal QtyOutstanding { get; set; }
        
        public decimal QtyReceived { get; set; }
        
        public decimal QtyPassedForPayment { get; set; }
    }
}
