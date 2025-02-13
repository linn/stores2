namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class RequisitionLine
    {
        protected RequisitionLine()
        {
        }

        public RequisitionLine(int? reqNumber, int? lineNumber)
        {
            if (reqNumber.HasValue)
            {
                this.ReqNumber = reqNumber.Value;
            }

            if (lineNumber.HasValue)
            {
                this.LineNumber = lineNumber.Value;
            }

            this.NominalAccountPostings = new List<RequisitionLinePosting>();
        }

        public RequisitionLine(int reqNumber, int lineNumber, Part part, decimal qty, StoresTransactionDefinition transaction)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;

            this.Part = part ?? throw new RequisitionException("Requisition line requires a part");
            this.TransactionDefinition = transaction ?? throw new RequisitionException("Requisition line requires a transaction");

            this.Qty = qty;

            this.Moves = new List<ReqMove>();

            this.NominalAccountPostings = new List<RequisitionLinePosting>();
            // TODO work out how to derive postings see STORES_OO.CREATENOMINALS and Post-Insert/RL trig in REQ UT

            this.Cancelled = "N";
        }

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

        public ICollection<StoresBudget> StoresBudgets { get; protected set; }

        public RequisitionHeader RequisitionHeader { get; set; }

        public void AddPosting(string debitOrCredit, decimal qty, NominalAccount nominalAccount)
        {
            if (!(debitOrCredit == "D" || debitOrCredit == "C"))
            {
                throw new RequisitionException("Debit or credit for posting should be D or C");
            }

            this.NominalAccountPostings.Add(new RequisitionLinePosting()
            {
                ReqNumber = this.ReqNumber,
                LineNumber = this.LineNumber,
                DebitOrCredit = debitOrCredit,
                Qty = qty,
                NominalAccount = nominalAccount
            });
        }

        public decimal GetPostingQty(string debitOrCredit)
        {
            return this.NominalAccountPostings.Where(p => p.DebitOrCredit == debitOrCredit && p.Qty != null).Sum(p => p.Qty.Value);
        }

        public bool IsCancelled() => this.DateCancelled != null || this.Cancelled == "Y";

        public bool IsBooked() => this.DateBooked != null;

        public bool HasDecrementTransaction() => this.TransactionDefinition?.IsDecrementTransaction ?? false;

        public bool HasMaterialVarianceTransaction() => this.TransactionDefinition?.MaterialVarianceTransaction ?? false;

        public void Cancel(int by, string reason, DateTime when)
        {
            if (this.IsBooked())
            {
                throw new RequisitionException("Cannot cancel a booked req line");
            }

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

        public bool OkToBook()
        {
            if (this.IsCancelled() || this.IsBooked())
            {
                return false;
            }

            var creditQty = this.GetPostingQty("D");
            var debitQty = this.GetPostingQty("C");

            if (creditQty != this.Qty || debitQty != this.Qty)
            {
                return false;
            }

            if (this.TransactionDefinition == null || !this.TransactionDefinition.RequiresMoves)
            {
                return true;
            }

            var moveQty = this.Moves.Sum(m => m.Quantity);
            return moveQty == this.Qty; // ensure moves have the qty 

            // some transactions dont require moves e.g. SUMVI

        }

        public bool RequiresAuthorisation()
        {
            if (this.IsCancelled() || this.IsBooked())
            {
                return false;
            }

            if (this.TransactionDefinition != null && this.TransactionDefinition.RequiresAuthorisation && this.Part != null)
            {
                // only have to authorise Finished Goods on transactions that require auth
                return this.Part.IsFinishedGoods();
            }

            return false;
        }

        public void Book(DateTime when)
        {
            this.DateBooked = when;
        }
    }
}
