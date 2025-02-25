namespace Linn.Stores2.Resources.Requisitions
{
    public class MoveFromResource
    {
            public string LocationCode { get; set; }
            
            public string LocationDescription { get; set; }
            
            public int? PalletNumber { get; set; }
            
            public string StockPool { get; set; }
            
            public string State { get; set; }
            
            public string BatchRef { get; set; }
            
            public string BatchDate { get; set; }
            
            public decimal? QtyAtLocation { get; set; }
            
            public decimal? QtyAllocated { get; set; }
    }
}
