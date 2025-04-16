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

        public RequisitionLine(
            int reqNumber, 
            int lineNumber, 
            Part part, 
            decimal qty, 
            StoresTransactionDefinition transaction,
            int? document1Number = null,
            int document1Line = 1,
            string document1Name = null)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Part = part;
            this.Booked = "N";
            if (this.Part == null)
            {
                throw new RequisitionException("Requisition line requires a part");
            }

            this.TransactionDefinition = transaction;
            if (this.TransactionDefinition == null)
            {
                throw new RequisitionException("Requisition line requires a transaction");
            }

            this.Qty = qty;

            if (this.Qty == 0)
            {
                throw new RequisitionException("Requisition line requires a qty");
            }

            this.Moves = new List<ReqMove>();

            this.NominalAccountPostings = new List<RequisitionLinePosting>();

            this.Cancelled = "N";
            
            this.Document1Number = document1Number ?? reqNumber;
            this.Document1Line = string.IsNullOrEmpty(document1Name) ? lineNumber: document1Line;
            this.Document1Type = string.IsNullOrEmpty(document1Name) ? "REQ" : document1Name;
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

        public string Booked { get; protected set; }

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
            return this.NominalAccountPostings == null ? 0 : this.NominalAccountPostings.Where(p => p.DebitOrCredit == debitOrCredit && p.Qty != null).Sum(p => p.Qty.Value);
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
            if (this.IsCancelled() || this.IsBooked() || this.Moves == null )
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

        public string AuthorisePrivilege()
        {
            if (this.RequiresAuthorisation() && this.TransactionDefinition != null)
            {
                return this.TransactionDefinition.AuthorisePrivilege();
            }

            return null;
        }

        public void Book(DateTime when)
        {
            this.DateBooked = when;
        }
    }
}
