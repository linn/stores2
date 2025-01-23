namespace Linn.Stores2.Resources.Requisitions
{
    public class MoveToResource
    {
        public int Seq { get; set; }
        
        public string LocationCode { get; set; }
            
        public string LocationDescription { get; set; }
            
        public int? PalletNumber { get; set; }
            
        public string StockPool { get; set; }
            
        public string State { get; set; } 
        
        public string SerialNumber { get; set; }

        public string Remarks { get; set; }
    }
}
