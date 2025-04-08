namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class PotentialMoveDetail
    {
        public int? ReqNumber { get; set; }

        public string PartNumber { get; set; }

        public decimal? Quantity { get; set; }

        public int? BuiltBy { get; set; }

        public int Sequence { get; set; }

        public string DocumentType { get; set; }

        public int DocumentId { get; set; }

        public int? LocationId { get; set; }

        public int? PalletNumber { get; set; }

        public int? SernosNumber { get; set; }

        public decimal? QuantityMoved { get; set; }
    }
}
