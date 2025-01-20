namespace Linn.Stores2.Resources.Requisitions
{
    public class RequisitionLineResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public string PartNumber { get; set; }
        
        public string PartDescription { get; set; }
        
        public decimal Qty { get; set; }

        public string TransactionCode { get; set; }
        
        public string TransactionCodeDescription { get; set; }

        public string DateCancelled { get; set; }

        public string CancelledReason { get; set; }

        public int? CancelledBy { get; set; }

        public int? Document1Line { get; set; }
        
        public int? Document1Number { get; set; }
    }
}
