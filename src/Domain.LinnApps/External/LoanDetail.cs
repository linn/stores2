namespace Linn.Stores2.Domain.LinnApps.External;

public class LoanDetail
{
    public int LineNumber { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public decimal Quantity { get; set; }
    
    public string ArticleNumber { get; set; }
}
