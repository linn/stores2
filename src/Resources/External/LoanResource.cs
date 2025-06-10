using System.Collections.Generic;

namespace Linn.Stores2.Resources.External;

public class LoanResource
{
    public int LoanNumber { get; set; }
    
    public string CancelledDate { get; set; }
    
    public IEnumerable<LoanDetailResource> LoanDetails { get; set; }
}
