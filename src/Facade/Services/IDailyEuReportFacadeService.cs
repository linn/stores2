namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IDailyEuReportFacadeService
    {
        Task<IResult<ReportReturnResource>> GetDailyEuImportRsnReport(string fromDate, string toDate);

        Task<IResult<ReportReturnResource>> GetDailyEuDespatchReport(string fromDate, string toDate);
    }
}
