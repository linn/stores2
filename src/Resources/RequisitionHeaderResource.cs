namespace Linn.Stores2.Resources
{
    using System;
    using System.Collections.Generic;

    public class RequisitionHeaderResource
    {
        public int ReqNumber { get; set; }

        public string DateCreated { get; set; }

        public int? Document1 { get;  set; }

        public IEnumerable<RequisitionLineResource> Lines { get;  set; }

        public decimal? Qty { get;  set; }

        public string Document1Name { get;  set; }

        public string PartNumber { get;  set; }

        public int? ToLocationId { get;  set; }

        public string ToLocation { get;  set; }

        public string Cancelled { get;  set; }

        public int? CancelledBy { get;  set; }

        public string DateCancelled { get;  set; }

        public string CancelledReason { get;  set; }

        public string FunctionCode { get;  set; }
    }
}
