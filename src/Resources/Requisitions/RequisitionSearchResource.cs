namespace Linn.Stores2.Resources.Requisitions
{
    public class RequisitionSearchResource
    {
        public int? ReqNumber { get; set; }

        public string Comments { get; set; }

        public bool IncludeCancelled { get; set; }

        public bool? Pending { get; set; }

        public string DocumentName { get; set; }

        public int? DocumentNumber { get; set; }
        
        public bool? BookedOnly { get; set; }

        public string FunctionCode { get; set; }

        public bool? ExcludeReversals { get; set; }
    }
}
