namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Collections.Generic;

    public class LoanResult
    {
        public int LoanNumber { get; set; }
        
        public bool IsCancelled { get; set; }
        
        public IEnumerable<LoanDetail> Details { get; set; }
    }
}
