namespace Linn.Stores2.Resources.Requisitions
{
    public class CancelRequisitionResource
    {
        public int ReqNumber { get; set; }

        public string Reason { get; set; }

        public int? LineNumber{ get; set; }
    }
}
