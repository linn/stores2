namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Stores;

    public class RequisitionLineResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public PartResource Part { get; set; }
        
        public decimal Qty { get; set; }

        public string TransactionCode { get; set; }
        
        public string TransactionCodeDescription { get; set; }

        public string DateCancelled { get; set; }

        public string CancelledReason { get; set; }

        public int? CancelledBy { get; set; }

        public int? Document1Line { get; set; }
        
        public int? Document1Number { get; set; }

        public string Document1Type { get; set; }

        public int? Document2Number { get; set; }

        public int? Document2Line { get; set; }

        public string Document2Type { get; set; }

        public string DateBooked { get; set; }

        public string Cancelled { get; set; }

        public IEnumerable<RequisitionLinePostingResource> Postings { get; set; }

        public IEnumerable<StoresBudgetResource> StoresBudgets { get; set; }

        public RequisitionHeaderResource RequisitionHeader { get; set; }
        
        public IEnumerable<MoveHeaderResource> Moves { get; set; }
    }
}
