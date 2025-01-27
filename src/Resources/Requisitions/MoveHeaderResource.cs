using System.Collections.Generic;

namespace Linn.Stores2.Resources.Requisitions
{
    public class MoveHeaderResource
    {
        public int Seq { get; set; }
        
        public string Part { get; set; }
        
        public decimal? Qty { get; set; }
        
        public string DateBooked { get; set; }
        
        public string DateCancelled { get; set; }
        
        public int ReqNumber { get; set; }
        
        public int LineNumber { get; set; }
        
        public MoveFromResource From { get; set; }
        
        public MoveToResource To { get; set; }
    }
}
