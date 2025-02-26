namespace Linn.Stores2.Resources.Requisitions
{
    public class MoveResource
    {
        public int Seq { get; set; }
        
        public string Part { get; set; }
        
        public decimal? Qty { get; set; }
        
        public string DateBooked { get; set; }
        
        public string DateCancelled { get; set; }
        
        public int ReqNumber { get; set; }
        
        public int LineNumber { get; set; }

        public string ToLocationCode { get; set; }

        public string ToLocationDescription { get; set; }

        public int? ToPalletNumber { get; set; }

        public string ToStockPool { get; set; }

        public string ToState { get; set; }

        public int? SerialNumber { get; set; }

        public string Remarks { get; set; }

        public string FromLocationCode { get; set; }

        public string FromLocationDescription { get; set; }

        public int? FromPalletNumber { get; set; }

        public string FromStockPool { get; set; }

        public string FromState { get; set; }

        public string FromBatchRef { get; set; }

        public string FromBatchDate { get; set; }

        public decimal? QtyAtLocation { get; set; }

        public decimal? QtyAllocated { get; set; }
    }
}
