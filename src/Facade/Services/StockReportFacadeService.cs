namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Resources;

    public class StockReportFacadeService : IStockReportFacadeService
    {
        private readonly IStockReportService stockReportService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        private readonly ICalcLabourHoursProxy labourHoursProxy;

        private readonly IPdfService pdfService;

        private readonly IHtmlTemplateService<LabourHoursInStockReport> htmlTemplateForLabourHoursInStock;

        private readonly IHtmlTemplateService<LabourHoursSummaryReport> htmlTemplateForLabourHoursSummary;

        public StockReportFacadeService(
            IStockReportService stockReportService,
            IReportReturnResourceBuilder reportResourceBuilder,
            ICalcLabourHoursProxy labourHoursProxy,
            IPdfService pdfService,
            IHtmlTemplateService<LabourHoursInStockReport> htmlTemplateForLabourHoursInStock,
            IHtmlTemplateService<LabourHoursSummaryReport> htmlTemplateForLabourHoursSummary)
        {
            this.stockReportService = stockReportService;
            this.reportResourceBuilder = reportResourceBuilder;
            this.labourHoursProxy = labourHoursProxy;
            this.pdfService = pdfService;
            this.htmlTemplateForLabourHoursInStock = htmlTemplateForLabourHoursInStock;
            this.htmlTemplateForLabourHoursSummary = htmlTemplateForLabourHoursSummary;
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

        public async Task<string> LabourHoursInStockAsHtml(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            var result = await this.stockReportService.GetStockInLabourHours(
                jobref,
                accountingCompany,
                includeObsolete);

            var total = await this.stockReportService.GetStockInLabourHoursTotal(
                jobref,
                accountingCompany,
                includeObsolete);

            var data = new LabourHoursInStockReport(result, total, jobref);

            return await this.htmlTemplateForLabourHoursInStock.GetHtml(data);
        }

        public async Task<Stream> LabourHoursInStockAsPdf(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            var html = await this.LabourHoursInStockAsHtml(jobref, accountingCompany, includeObsolete);

            return await this.pdfService.ConvertHtmlToPdf(html, false);
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

        public async Task<string> LabourHourSummaryAsHtml(string fromDate, string toDate, string accountingCompany = "LINN")
        {
            if (!DateTime.TryParse(fromDate, out var from) || !DateTime.TryParse(toDate, out var to))
            {
                return "Invalid date format for fromDate or toDate.";
            }

            var result = await this.stockReportService.GetLabourHoursSummaryReport(
                from, to,
                accountingCompany);

            var data = new LabourHoursSummaryReport
            {
                AccountingCompany = accountingCompany,
                FromDate = from,
                ToDate = to,
                ReportDate = DateTime.UtcNow,
                Report = result.First(),
                TotalsReport = result.Last()
            };

            return await this.htmlTemplateForLabourHoursSummary.GetHtml(data);
        }

        public async Task<Stream> LabourHourSummaryAsPdf(string fromDate, string toDate, string accountingCompany = "LINN")
        {
            var html = await this.LabourHourSummaryAsHtml(fromDate, toDate, accountingCompany);

            return await this.pdfService.ConvertHtmlToPdf(html, true);
        }

        public async Task<IResult<ReportReturnResource>> LabourHoursInLoans()
        {
            var result = await this.stockReportService.GetLabourHoursInLoans();

            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }
    }
}
