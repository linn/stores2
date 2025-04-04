namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    public interface ISalesProxy
    {
        Task<SalesArticleResult> GetSalesArticle(string partNumber);
    }
}
