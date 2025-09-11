namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;

    public interface IDailyEuReportFacdeService
    {
        Task<ResultsModel> GetDailyEuImportRsnReport(string fromDate, string toDate);

        Task<ResultsModel> GetDailyEuDespatchReport(string fromDate, string toDate);
    }
}
