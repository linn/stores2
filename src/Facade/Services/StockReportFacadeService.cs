namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Resources;

    public class StockReportFacadeService : IStockReportFacadeService
    {
        private readonly IStockReportService stockReportService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        private readonly IPdfService pdfService;

        public StockReportFacadeService(IStockReportService stockReportService,
            IReportReturnResourceBuilder reportResourceBuilder,
            IPdfService pdfService)
        {
            this.stockReportService = stockReportService;
            this.reportResourceBuilder = reportResourceBuilder;
            this.pdfService = pdfService;
        }

        public async Task<IResult<ReportReturnResource>> LabourHoursInStock(string jobref,
            string accountingCompany = "LINN",
        bool includeObsolete = true)
        {
            var result = await stockReportService.GetStockInLabourHours(jobref, accountingCompany, includeObsolete);

            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }

        public async Task<IResult<TotalResource>> LabourHoursInStockTotal(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            var total = await stockReportService.GetStockInLabourHoursTotal(jobref, accountingCompany, includeObsolete);

            return new SuccessResult<TotalResource>(new TotalResource(total));
        }

        public async Task<IResult<ReportReturnResource>> LabourHourSummary(string fromDate, string toDate, string accountingCompany = "LINN")
        {
            if (!DateTime.TryParse(fromDate, out var from) || !DateTime.TryParse(toDate, out var to))
            {
                return new BadRequestResult<ReportReturnResource>("Invalid date format for fromDate or toDate.");
            }

            var result = await stockReportService.GetLabourHoursSummaryReport(from, to, accountingCompany);
            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }
    }
}
