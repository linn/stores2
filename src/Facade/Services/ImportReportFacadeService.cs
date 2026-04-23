namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Domain.LinnApps.Imports;

    public class ImportReportFacadeService : IImportReportFacadeService
    {
        private readonly IImportReportService importReportService;

        private readonly IImportBookUploadService importBookUploadService;

        private readonly IPdfService pdfService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public ImportReportFacadeService(
            IImportReportService importReportService, IImportBookUploadService importBookUploadService, IPdfService pdfService, IReportReturnResourceBuilder resourceBuilder)
        {
            this.importReportService = importReportService;
            this.importBookUploadService = importBookUploadService;
            this.pdfService = pdfService;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<StreamResponse>> GetClearanceInstructionAsPdf(
            IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var html = await this.importReportService.GetClearanceInstructionAsHtml(impbookIds, toEmailAddress);

            var pdf = await this.pdfService.ConvertHtmlToPdf(html, false);

            return new SuccessResult<StreamResponse>(new StreamResponse
            {
                Stream = pdf,
                ContentType = "application/pdf",
                FileName = $"ImportClearanceInstruction.pdf"
            });
        }

        public async Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var htmlResult = await this.importReportService.GetClearanceInstructionAsHtml(impbookIds, toEmailAddress);

            return htmlResult;
        }

        public async Task<IResult<ReportReturnResource>> GetImportBookComparerReport(string fromDate, string toDate, Stream csvData)
        {
            var results = await this.importBookUploadService.UploadImportBookDetailCsvAsync(DateTime.Parse(fromDate), DateTime.Parse(toDate), csvData);

            //var result = await this.importReportService.CompareImportBooksWithCsvReport(DateTime.Parse(toDate), DateTime.Parse(fromDate), csvData);

            var a = new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(results));

            return a;
        }
    }
}
