namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;

    // Represents a temporary data structure for a requisition line before it is processed by  
    // the domain service or factory methods and converted into a persisted line entity as part of a requistion.
    public class LineCandidate
    {
        public IEnumerable<MoveSpecification> Moves { get; set; }

        public int LineNumber { get; set; }

        public string PartNumber { get; set; }

        public int? Document1 { get; set; }

        public int? Document1Line { get; set; }

        public string Document1Type { get; set; }

        public decimal Qty { get; set; }

        public string TransactionDefinition { get; set; }
        
        public bool? StockPicked { get; set; }

        public IEnumerable<int> SerialNumbers { get; set; }
        
        public string Cancelled { get; set; }
    }
}
