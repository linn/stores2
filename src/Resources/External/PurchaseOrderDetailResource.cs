namespace Linn.Stores2.Resources.External
{
    public class PurchaseOrderDetailResource
    {
        public int Line { get; set; }

        public string PartNumber { get; set; }

        public decimal OurQty { get; set; }

        public int? OriginalOrderNumber { get; set; }
    }
}
