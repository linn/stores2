namespace Linn.Stores2.Resources.External
{
    public class PurchaseOrderDeliveryResource
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySeq { get; set; }

        public decimal OurDeliveryQty { get; set; }

        public decimal QuantityOutstanding { get; set; }
        
        public decimal QtyNetReceived { get; set; }
        
        public decimal QtyPassedForPayment { get; set; }
    }
}
