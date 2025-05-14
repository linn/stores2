namespace Linn.Stores2.Domain.LinnApps.External
{
    public class PurchaseOrderDeliveryResult
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public decimal OurQuantity { get; set; }

        public decimal QuantityOutstanding { get; set; }
        
        public decimal QuantityReceived { get; set; }
        
        public decimal QuantityPassedForPayment { get; set; }
    }
}
