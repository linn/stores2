﻿namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class MoveSpecification
    {
        public decimal Qty { get; set; }

        public string FromLocation { get; set; }

        public int? FromPallet { get; set; }

        public string FromState { get; set; }

        public string ToLocation { get; set; }
        
        public int? ToLocationId { get; set; }
        
        public int? ToPallet { get; set; }
        
        public string ToStockPool { get; set; }
        
        public string ToState { get; set; }

        public string FromStockPool { get; set; }
        
        public bool IsAddition { get; set; }
    }
}
