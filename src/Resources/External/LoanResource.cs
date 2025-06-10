namespace Linn.Stores2.Resources.External
{
    using System.Collections.Generic;

    public class LoanResource
    {
        public int LoanNumber { get; set; }
    
        public string CancelledDate { get; set; }
    
        public IEnumerable<LoanDetailResource> LoanDetails { get; set; }
    }

}
