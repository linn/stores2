namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class MoveSpecification
    {
        public string PartNumber { get; set; }

        public decimal Qty { get; set; }

        public string Location { get; set; }

        public int? Pallet { get; set; }
    }
}
