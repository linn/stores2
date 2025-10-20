namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class DailyEuReportsFacadeService : IDailyEuReportFacdeService
    {
        private readonly IDailyEuReportService dailyEuReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public DailyEuReportsFacadeService(
            IDailyEuReportService dailyEuReportService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.dailyEuReportService = dailyEuReportService;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<ReportReturnResource>> GetDailyEuDespatchReport(string fromDate, string toDate)
        {
            var result = await this.dailyEuReportService.GetDailyEuDespatchReport(fromDate, toDate);

            var a = new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));

            return a;
        }

        public async Task<IResult<ReportReturnResource>> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            var result = await this.dailyEuReportService.GetDailyEuImportRsnReport(fromDate, toDate);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }
    }
}