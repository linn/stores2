namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class DailyEuReportsFacadeService : IDailyEuReportFacadeService
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

        public async Task<IResult<ReportReturnResource>> GetDailyEuDispatchReport(string fromDate, string toDate)
        {
            var fromDateForService = DateTime.Parse(fromDate);
            var toDateForService = DateTime.Parse(toDate);
            var result = await this.dailyEuReportService.GetDailyEuDispatchReport(fromDateForService, toDateForService);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }

        public async Task<IResult<ReportReturnResource>> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            var result = await this.dailyEuReportService.GetDailyEuRsnImportReport(fromDate, toDate);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }

        public async Task<IResult<ReportReturnResource>> GetDailyEuDispatchRsnReport(string fromDate, string toDate)
        {
            var fromDateForService = DateTime.Parse(fromDate);
            var toDateForService = DateTime.Parse(toDate);
            var result =
                await this.dailyEuReportService.GetDailyEuRsnDispatchReport(fromDateForService, toDateForService);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }
    }
}
