namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System;
    using System.Collections.Generic;

    public class RequisitionCreationContext
    {
        public StoresFunction Function { get; set; }

        public int CreatedByUserNumber { get; set; }

        public IEnumerable<string> UserPrivileges { get; set; }

        public string ReqType { get; set; }

        public int? Document1Number { get; set; }

        public string Document1Type { get; set; }

        public int? Document1Line { get; set; }

        public int? Document2Number { get; set; }

        public string Document2Type { get; set; }

        public string DepartmentCode { get; set; }

        public string NominalCode { get; set; }

        public string Reference { get; set; }

        public string Comments { get; set; }

        public string ManualPick { get; set; }

        public string FromStockPool { get; set; }

        public string ToStockPool { get; set; }

        public string FromLocationCode { get; set; }

        public string ToLocationCode { get; set; }

        public string PartNumber { get; set; }

        public string NewPartNumber { get; set; }

        public decimal? Quantity { get; set; }

        public string FromState { get; set; }

        public string ToState { get; set; }

        public int? FromPallet { get; set; }

        public int? ToPallet { get; set; }

        public DateTime? BatchDate { get; set; }

        public string BatchRef { get; set; }

        public IEnumerable<LineCandidate> Lines { get; set; }

        public int? OriginalReqNumber { get; set; }

        public string IsReverseTransaction { get; set; }

        public int? Document3Number { get; set; }
        
        public IEnumerable<BookInOrderDetail> BookInOrderDetails { get; set; }

        public DateTime? DateReceived { get; set; }
        
        public string FromCategory { get; set; }
    }
}
