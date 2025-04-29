namespace Linn.Stores2.Domain.LinnApps.External
{
    public class PurchaseOrderDetailResult
    {
        public int Line { get; set; }

        public string PartNumber { get; set; }

        public decimal OurQty { get; set; }

        public int? OriginalOrderNumber { get; set; }
    }
}
