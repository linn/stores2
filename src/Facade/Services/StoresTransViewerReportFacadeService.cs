namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class StoresTransViewerReportFacadeService : IStoresTransViewerReportFacadeService
    {
        private readonly IStoresTransViewerReportService storesTransViewerReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        private readonly IStringFromFileService stringFromFileService;

        private readonly IPdfService pdfService;

        private readonly IHtmlTemplateService<StoresTransactionReport> htmlTemplateServiceForStoresTransaction;

        public StoresTransViewerReportFacadeService(
            IStoresTransViewerReportService storesTransViewerReportService,
            IReportReturnResourceBuilder resourceBuilder,
            IStringFromFileService stringFromFileService,
            IPdfService pdfService, 
            IHtmlTemplateService<StoresTransactionReport> htmlTemplateServiceForStoresTransaction)
        {
            this.storesTransViewerReportService = storesTransViewerReportService;
            this.resourceBuilder = resourceBuilder;
            this.stringFromFileService = stringFromFileService;
            this.pdfService = pdfService;
            this.htmlTemplateServiceForStoresTransaction = htmlTemplateServiceForStoresTransaction;
        }

        public async Task<IResult<ReportReturnResource>> GetStoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList)
        {
            var result = await this.storesTransViewerReportService.StoresTransViewerReport(
                fromDate,
                toDate,
                partNumber, 
                transactionCode,
                functionCodeList);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }

        public async Task<Stream> GetStoresTransactionReportAsPdf(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList)
        {
            var report = this.storesTransViewerReportService.StoresTransViewerReport(
                fromDate,
                toDate,
                partNumber,
                transactionCode,
                functionCodeList);

            var data = new StoresTransactionReport(report.Result);
            var html = await this.htmlTemplateServiceForStoresTransaction.GetHtml(data);
            var footerHtml = await this.stringFromFileService.GetString("Footer.html");

            return await this.pdfService.ConvertHtmlToPdf(html, true);
        }
    }
}
