namespace Linn.Stores2.Resources.Requisitions
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Resources;
    using Linn.Stores2.Resources.Accounts;

    public class RequisitionHeaderResource : HypermediaResource
    {
        public int ReqNumber { get; set; }

        public string DateCreated { get; set; }

        public int? Document1 { get; set; }

        public IEnumerable<RequisitionLineResource> Lines { get; set; }

        public decimal? Qty { get; set; }

        public string Document1Name { get; set; }

        public string PartNumber { get; set; }
        
        public int? ToLocationId { get; set; }

        public string ToLocationCode { get; set; }

        public string FromLocationCode { get; set; }

        public int? ToPalletNumber { get; set; }

        public int? FromPalletNumber { get; set; }

        public string Cancelled { get; set; }

        public int? CancelledBy { get; set; }
        
        public string CancelledByName { get; set; }

        public string DateCancelled { get; set; }

        public string CancelledReason { get; set; }

        public FunctionCodeResource FunctionCode { get; set; }
        
        public string Comments { get; set; }
        
        public int? BookedBy { get; set; }
        
        public string BookedByName { get; set; }
        
        public string DateBooked { get; set; }
        
        public int? CreatedBy { get; set; }
        
        public string CreatedByName { get; set; }
        
        public string Reversed { get; set; }

        public int? AuthorisedBy { get; set; }

        public string AuthorisedByName { get; set; }

        public string DateAuthorised { get; set; }

        public NominalResource Nominal { get; set; }

        public DepartmentResource Department { get; set; }

        public string ReqType { get; set; }

        public string ManualPick { get; set; }

        public string Reference { get; set; }

        public string FromStockPool { get; set; }

        public string ToStockPool { get; set; }
    }
}
