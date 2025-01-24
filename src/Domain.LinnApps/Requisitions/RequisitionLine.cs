namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;

    using Linn.Stores2.Domain.LinnApps.Parts;

    public class RequisitionLine
    {
        public int ReqNumber { get; protected init; }

        public int LineNumber { get; protected init; }

        public Part Part { get; protected set; }
        
        public DateTime? DateCancelled { get; protected set; }

        public string CancelledReason { get; protected set; }

        public int? CancelledBy { get; protected set; }

        public int? Document1Number { get; protected set; }

        public int? Document1Line { get; protected set; }

        public string Document1Type { get; protected set; }

        public int? Document2Number { get; protected set; }

        public int? Document2Line { get; protected set; }

        public string Document2Type { get; protected set; }

        public ICollection<ReqMove> Moves { get; protected set; }
        
        public decimal Qty { get; protected set; }
        
        public StoresTransactionDefinition TransactionDefinition { get; protected set; }

        public string Cancelled { get; protected set; }

        public DateTime? DateBooked { get; protected set; }

        public ICollection<RequisitionLinePosting> NominalAccountPostings { get; protected set; }

        public void Cancel(int by, string reason, DateTime when)
        {
            this.CancelledBy = by;
            this.Cancelled = "Y";
            this.CancelledReason = reason;
            this.DateCancelled = when;

            if (this.Moves == null)
            {
                return;
            }

            foreach (var reqMove in this.Moves)
            {
                reqMove.Cancel(when);
            }
        }
    }
}
