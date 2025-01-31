namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionHeader
    {
        public int ReqNumber { get; protected init; }

        public DateTime DateCreated { get; protected init; }
        
        public Employee CreatedBy { get; protected init; }

        public int? Document1 { get; protected set; }

        public ICollection<RequisitionLine> Lines { get; protected set; }
        
        public decimal? Qty { get; protected set; }

        public string Document1Name { get; protected set; }
        
        public Part Part { get; protected set; }

        public StorageLocation ToLocation { get; protected set; }

        public StorageLocation FromLocation { get; protected set; }

        public int? FromPalletNumber { get; protected set; }

        public int? ToPalletNumber { get; protected set; }

        public string Cancelled { get; protected set; }

        public Employee CancelledBy { get; protected set; }

        public DateTime? DateCancelled { get; protected set; }

        public string CancelledReason { get; protected set; }
        
        public StoresFunctionCode FunctionCode { get; protected set;}
        
        public string Comments { get; protected set; }
        
        public DateTime? DateBooked { get; protected set; }
        
        public Employee BookedBy { get; protected set; }
        
        public string Reversed { get; protected set; }

        public ICollection<CancelDetails> CancelDetails { get; protected set; }

        public Department Department { get; protected set; }

        public Nominal Nominal { get; protected set; }

        public Employee AuthorisedBy { get; protected set; }

        public DateTime? DateAuthorised { get; protected set;  }

        public string ManualPick { get; protected set; }

        public string ReqType { get; set; }

        public string Reference { get; set; }

        public string FromStockPool { get; set; }

        public string ToStockPool { get; set; }

        public RequisitionHeader()
        {
        }

        public RequisitionHeader(
            Employee createdBy,
            StoresFunctionCode functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            Department department,
            Nominal nominal, 
            IEnumerable<RequisitionLine> lines = null,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            StorageLocation fromLocation = null,
            StorageLocation toLocation = null)
        {
            this.Comments = comments;
            this.DateCreated = DateTime.Now;
            this.FunctionCode = functionCode;
            this.Document1 = document1Number;
            this.Document1Name = document1Type;

            if (lines != null)
            {
                foreach (var l in lines)
                {
                    this.AddLine(l);
                }
            }

        }

        public void AddLine(RequisitionLine toAdd)
        {
            this.Lines ??= new List<RequisitionLine>();
            toAdd.RequisitionHeader = this;
            this.Lines.Add(toAdd);
        }

        public void Book(Employee bookedBy)
        {
            this.DateBooked = DateTime.Now;
            this.BookedBy = bookedBy;
        }

        public void BookLine(int lineNumber, Employee bookedBy)
        {
            this.Lines.First(x => x.LineNumber == lineNumber).Book();

            if (!this.DateBooked.HasValue && this.Lines.All(l => l.DateBooked.HasValue))
            {
                this.Book(bookedBy);
            }
        }

        public void Cancel(string reason, Employee cancelledBy)
        {
            // note: this function does not represent a complete picture
            // a lot of extra logic surrounding cancelling the req is in stored procedures
            // see RequisitionService
            if (string.IsNullOrEmpty(reason))
            {
                throw new RequisitionException("Must provide a cancel reason");
            }

            if (this.DateBooked.HasValue)
            {
                throw new RequisitionException("Cannot cancel a booked req");
            }

            var now = DateTime.Now;
            this.Cancelled = "Y";
            this.CancelledBy = cancelledBy;
            this.DateCancelled = now;
            this.CancelledReason = reason;

            this.CancelDetails ??= new List<CancelDetails>();

            this.CancelDetails.Add(new CancelDetails
                                       {
                                           ReqNumber = this.ReqNumber,
                                           DateCancelled = now,
                                           Reason = reason,
                                           CancelledBy = cancelledBy.Id
                                       });
            if (this.Lines == null)
            {
                return;
            }

            foreach (var l in this.Lines)
            {
                // Set some cancelled fields
                // This probably happens in the store procedures anyway
                // But no harm in making sure
                // (and actually updating this tracked entity so updates can be returned to the client)
                l.Cancel(cancelledBy.Id, reason, now);
            }
        }

        public void CancelLine(int lineNumber, string reason, Employee cancelledBy)
        {
            var now = DateTime.Now;
            
            // cancel line
            this.Lines.First(x => x.LineNumber == lineNumber).Cancel(cancelledBy.Id, reason, now);

            // cancel header if all lines are now cancelled
            if (this.Lines.All(x => x.DateCancelled.HasValue) 
                && !this.DateCancelled.HasValue)
            {
                this.Cancel(reason, cancelledBy);
            }

            // book header if all non-cancelled lines are booked
            if (!this.DateBooked.HasValue
                && this.Lines.Where(x => !x.DateCancelled.HasValue).All(l => l.DateBooked.HasValue))
            {
                this.DateBooked = now;
            }
        }
    }
}
