namespace Linn.Stores2.Resources
{
    public class RequisitionLineResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public string PartNumber { get; set; }

        public string TransactionCode { get; set; }

        public string DateCancelled { get; set; }

        public string CancelledReason { get; set; }

        public int? CancelledBy { get; set; }

        public int? Document1Line { get; set; }
    }
}
