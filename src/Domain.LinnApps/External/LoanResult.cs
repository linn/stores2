using System.Collections.Generic;

namespace Linn.Stores2.Domain.LinnApps.External
{
    public class LoanResult
    {
        public int LoanNumber { get; set; }
        
        public bool IsCancelled { get; set; }
        
        public IEnumerable<LoanDetail> Details { get; set; }
    }
}
