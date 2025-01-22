﻿namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    
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

        public ICollection<ReqMove> Moves { get; protected set; }

        public decimal? Qty { get; protected set; }

        public string Document1Name { get; protected set; }

        public string PartNumber { get; protected set; }

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

        public RequisitionHeader(int? reqNumber, string comments, StoresFunctionCode functionCode)
        {
            if (reqNumber.HasValue)
            {
                this.ReqNumber = reqNumber.Value;
            }

            this.Comments = comments;
            this.DateCreated = DateTime.Now;
            this.FunctionCode = functionCode;
        }

        public void Cancel(string reason, Employee cancelledBy)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new RequisitionException("Must provide a cancel reason");
            }

            var now = DateTime.Now;
            this.Cancelled = "Y";
            this.CancelledBy = cancelledBy;
            this.DateCancelled = now;
            this.CancelledReason = reason;

            this.CancelDetails ??= new List<CancelDetails>();

            // todo - will need to make sure Id is set at the db level
            this.CancelDetails.Add(new CancelDetails
                                       {
                                           ReqNumber = this.ReqNumber,
                                           DateCancelled = now,
                                           Reason = reason,
                                           CancelledBy = cancelledBy.Id
                                       });
        }
    }
}
