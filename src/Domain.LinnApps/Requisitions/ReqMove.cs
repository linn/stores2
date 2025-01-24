namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Stock;

    public class ReqMove
    {
        public int ReqNumber { get; protected init; }

        public int LineNumber { get; protected init; }

        public int Sequence { get; protected init; }

        public decimal Quantity { get; protected set; }

        public int? StockLocatorId { get; protected set; }

        public StockLocator StockLocator { get; protected set; }

        public int? PalletNumber { get; protected set; }

        public int? LocationId { get; protected set; }

        public StorageLocation Location { get; protected set; }

        public string StockPoolCode { get; protected set; }

        public string Booked { get; protected set; }

        public string Remarks { get; protected set; }

        public RequisitionHeader Header { get; protected set; }

        public DateTime? DateBooked { get; protected set; }

        public DateTime? DateCancelled { get; protected set; }
        
        public string State { get; protected set; }
        
        public int? SerialNumber { get; protected set; }

        public void Cancel(DateTime when)
        {
            this.DateCancelled = when;
        }
    }
}
