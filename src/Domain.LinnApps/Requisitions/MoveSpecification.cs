namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class MoveSpecification
    {
        public decimal Qty { get; set; }

        public string FromLocation { get; set; }

        public int? FromPallet { get; set; }

        public string ToLocation { get; set; }

        public int? ToPallet { get; set; }
    }
}
