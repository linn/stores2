namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
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

        private readonly IPdfService pdfService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public ImportReportFacadeService(
            IImportReportService importReportService, IPdfService pdfService, IReportReturnResourceBuilder resourceBuilder)
        {
            this.importReportService = importReportService;
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

        public async Task<IResult<ReportReturnResource>> GetImportBookComparerReport(string toDate, string fromDate, List<string> customsEntryCodes)
        {
            var result = await this.importReportService.GetImportBookComparerReport(DateTime.Parse(toDate), DateTime.Parse(fromDate), customsEntryCodes);

            var a = new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));

            return a;
        }
    }
}
