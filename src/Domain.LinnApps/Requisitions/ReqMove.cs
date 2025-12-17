namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class ReqMove
    {
        public ReqMove()
        {
        }

        public ReqMove(
            int reqNumber,
            int lineNumber,
            int seq,
            decimal qty,
            int? stockLocatorId,
            int? palletNumber,
            int? locationId,
            string stockPool,
            string state,
            string category,
            StockLocator stockLocator = null)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Sequence = seq;
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
            this.StockLocator = stockLocator;
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

        public void UnpickQuantity(decimal unpickQty)
        {
            if (unpickQty < this.Quantity)
            {
                this.Quantity -= unpickQty;
            }
            else if (unpickQty == this.Quantity)
            {
                // previous req_ut.fmb did this if unpicking full quantity in UNPICK_STOCK
                this.Cancel(DateTime.Now);
            }
        }

        public bool HasValidAllocation() => (this.StockLocatorId != null);

        public bool HasValidOnto() => (this.PalletNumber != null || this.LocationId != null) && !string.IsNullOrEmpty(this.StockPoolCode) && !string.IsNullOrEmpty(this.State) && !string.IsNullOrEmpty(this.Category);

        public bool IsCancelled() => this.DateCancelled != null;

        public bool IsBooked() => this.DateBooked != null || this.Booked == "Y";

        public bool CanUnPick() => !this.IsBooked() && !this.IsCancelled() && this.StockLocator != null;

        public bool MoveIsBookable(StoresTransactionDefinition transaction)
        {
            if (!this.IsBooked() && !this.IsCancelled())
            {
                var canBeBooked = this.MoveCanBeBooked(transaction);
                return canBeBooked.Success;
            }

            return false;
        }

        public ProcessResult MoveCanBeBooked(StoresTransactionDefinition transaction)
        {
            if (transaction.RequiresStockAllocations && !this.HasValidAllocation())
            {
                return new ProcessResult(
                    false,
                    $"Move {this.Sequence} on line {this.LineNumber} does not have a valid allocation.");
            }

            if (transaction.RequiresOntoTransactions && !this.HasValidOnto())
            {
                return new ProcessResult(
                    false,
                    $"Move {this.Sequence} on line {this.LineNumber} does not have a valid onto.");
            }

            return new ProcessResult(true, $"{this.Sequence} can be booked");
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

        public string TextSummary()
        {
            var textSummary = $"{this.Quantity} ";
            if (this.StockLocator != null)
            {
                if (this.StockLocator.StorageLocation != null)
                {
                    textSummary += $"From {this.StockLocator.StorageLocation.LocationCode} ";
                } else if (this.StockLocator.PalletNumber.HasValue)
                {
                    textSummary += $"From Pallet {this.StockLocator.PalletNumber.Value} ";
                }
            }

            if (this.Location != null)
            {
                textSummary += $"To {this.Location.LocationCode}";
            }
            else if (this.PalletNumber.HasValue)
            {
                textSummary += $"To Pallet {this.PalletNumber.Value}";
            }

            return textSummary;
        }
    }
}
