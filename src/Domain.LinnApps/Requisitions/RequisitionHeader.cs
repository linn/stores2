namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionHeader
    {
        public int ReqNumber { get; protected init; }

        public DateTime DateCreated { get; protected init; }

        public int? Document1 { get; protected set; }

        public ICollection<RequisitionLine> Lines { get; protected set; }

        public ICollection<ReqMove> Moves { get; protected set; }

        public decimal? Qty { get; protected set; }

        public string Document1Name { get; protected set; }

        public string PartNumber { get; protected set; }

        public Part Part { get; protected set; }

        public int? ToLocationId { get; protected set; }

        public StorageLocation ToLocation { get; protected set; }

        public string Cancelled { get; protected set; }

        public int? CancelledBy { get; protected set; }

        public DateTime? DateCancelled { get; protected set; }

        public string CancelledReason { get; protected set; }

        public string FunctionCode { get; protected set; }

        public string Comments { get; protected set; }

        public RequisitionHeader()
        {
        }

        public RequisitionHeader(int? reqNumber, string comments)
        {
            if (reqNumber.HasValue)
            {
                this.ReqNumber = reqNumber.Value;
            }

            this.Comments = comments;
        }

        public void Cancel(int cancelledBy)
        {
            this.CancelledBy = cancelledBy;
            this.DateCancelled = DateTime.Now;
        }
    }
}
