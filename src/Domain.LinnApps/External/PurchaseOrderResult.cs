namespace Linn.Stores2.Domain.LinnApps.External
{
    public class PurchaseOrderResult
    {
        public int OrderNumber { get; set; }

        public bool IsFilCancelled { get; set; }

        public bool IsAuthorised { get; set; }
        
        public PurchaseOrderType DocumentType { get; set; }
    }
}
