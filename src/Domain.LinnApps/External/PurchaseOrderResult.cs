namespace Linn.Stores2.Domain.LinnApps.External
{
    public class PurchaseOrderResult
    {
        public int OrderNumber { get; set; }

        public bool IsFilCancelled { get; set; }

        public bool IsAuthorised { get; set; }
        
        public string DocumentType { get; set; }

        // todo - populate
        public int SupplierId { get; set; }

        public int SupplierName { get; set; }

        public string RohsCompliant { get; set; }
    }
}
