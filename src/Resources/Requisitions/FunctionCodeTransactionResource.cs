namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    public class FunctionCodeTransactionResource
    {
        public int Seq { get; set; }
        
        public string ReqType { get; set; }
        
        public string TransactionDefinition { get; set; }
        
        public string TransactionDescription { get; set; }

        public bool? StockAllocations { get; set; }

        public IEnumerable<string> FromStates { get; set; }

        public IEnumerable<string> ToStates { get; set; }
    }
}
