namespace Linn.Stores2.Resources.Requisitions
{
    public class UnpickRequisitionResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public int Seq { get; set; }

        public decimal QtyToUnpick { get; set; }

        public bool? Reallocate { get; set; }
    }
}
