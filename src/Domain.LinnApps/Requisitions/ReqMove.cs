namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Stock;

    public class ReqMove
    {
        public ReqMove()
        {
                
        }

        public ReqMove(int reqNumber , int lineNumber, int seq, decimal qty, int? stockLocatorId, int? palletNumber, int? locationId, string stockPool, string state, string category)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Sequence = Sequence;
            this.Quantity = qty;
            this.StockLocatorId = stockLocatorId;
            this.PalletNumber = palletNumber;
            this.LocationId = locationId;
            this.StockPoolCode = stockPool;
            this.State = state;
            this.Category = category;
            this.DateBooked = null;
            this.DateCancelled = null;
            this.Booked = "N";
        }

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

        public string Category { get; protected set; }

        public int? SerialNumber { get; protected set; }

        public void Cancel(DateTime when)
        {
            this.DateCancelled = when;
        }

        public void Book(DateTime when)
        {
            this.DateBooked = when;
            this.Booked = "Y";
        }

        public bool HasValidAllocation() => (this.StockLocatorId != null);

        public bool HasValidOnto() => (this.PalletNumber != null || this.LocationId != null) && !string.IsNullOrEmpty(this.StockPoolCode) && !string.IsNullOrEmpty(this.State) && !string.IsNullOrEmpty(this.Category);

        public bool IsCancelled() => this.DateCancelled != null;

        public bool IsBooked() => this.DateBooked != null || this.Booked == "Y";

        public bool OkToBook(StoresTransactionDefinition transaction)
        {
            if (!this.IsBooked() && !this.IsCancelled())
            {
                if (transaction.RequiresStockAllocations)
                {
                    return this.HasValidAllocation();
                }

                if (transaction.RequiresOntoTransactions)
                {
                    return this.HasValidOnto();
                }
            }

            return false;
        }

        public void SetOntoFieldsFromHeader(RequisitionHeader header)
        {
            if (header != null)
            {
                if (header.ToLocation != null)
                {
                    this.Location = header.ToLocation;
                    this.LocationId = header.ToLocation.LocationId;
                }

                if (header.ToPalletNumber != null)
                {
                    this.PalletNumber = header.ToPalletNumber;
                }

                if (!string.IsNullOrEmpty(header.ToStockPool))
                {
                    this.StockPoolCode = header.ToStockPool;
                }

                if (!string.IsNullOrEmpty(header.ToState))
                {
                    this.State = header.ToState;
                }

                if (!string.IsNullOrEmpty(header.ToCategory))
                {
                    this.Category = header.ToCategory;
                }
            }
        }
    }
}
