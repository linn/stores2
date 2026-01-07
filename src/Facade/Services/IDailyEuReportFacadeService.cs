namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IDailyEuReportFacadeService
    {
        Task<IResult<ReportReturnResource>> GetDailyEuRsnImportReport(string fromDate, string toDate);

        Task<IResult<ReportReturnResource>> GetDailyEuDispatchReport(string fromDate, string toDate);

        Task<IResult<ReportReturnResource>> GetDailyEuRsnDispatchReport(string fromDate, string toDate);
    }
}
