namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Resources;

    public class StockReportFacadeService : IStockReportFacadeService
    {
        private readonly IStockReportService stockReportService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        private readonly ICalcLabourHoursProxy labourHoursProxy;

        public StockReportFacadeService(
            IStockReportService stockReportService,
            IReportReturnResourceBuilder reportResourceBuilder,
            ICalcLabourHoursProxy labourHoursProxy)
        {
            this.stockReportService = stockReportService;
            this.reportResourceBuilder = reportResourceBuilder;
            this.labourHoursProxy = labourHoursProxy;
        }

        public async Task<IResult<ReportReturnResource>> LabourHoursInStock(
            string jobref,
            string accountingCompany = "LINN",
        bool includeObsolete = true)
        {
            var result = await this.stockReportService.GetStockInLabourHours(
                             jobref,
                             accountingCompany,
                             includeObsolete);

            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }

        public async Task<IResult<TotalResource>> LabourHoursInStockTotal(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true)
        {
            var total = await this.stockReportService.GetStockInLabourHoursTotal(
                            jobref,
                            accountingCompany,
                            includeObsolete);

            return new SuccessResult<TotalResource>(new TotalResource(total));
        }

        public async Task<IResult<ReportReturnResource>> LabourHourSummary(
            string fromDate,
            string toDate,
            string accountingCompany = "LINN",
            bool recalcLabourTimes = false)
        {
            if (!DateTime.TryParse(fromDate, out var from) || !DateTime.TryParse(toDate, out var to))
            {
                return new BadRequestResult<ReportReturnResource>("Invalid date format for fromDate or toDate.");
            }

            if (recalcLabourTimes)
            {
                await this.labourHoursProxy.CalcLabourTimes();
            }

            var result = await this.stockReportService.GetLabourHoursSummaryReport(from, to, accountingCompany);
            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }
    }
}
