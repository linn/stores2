namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    // Represents a specification for a stock move before it is processed 
    // by stores_oo.pick_stock, where the actual moves will be determined and returned.
    public class MoveSpecification
    {
        public string PartNumber { get; set; }

        public decimal Qty { get; set; }

        public string FromLocation { get; set; }

        public int? FromPallet { get; set; }
    }
}
