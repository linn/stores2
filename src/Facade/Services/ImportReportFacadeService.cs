namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Domain.LinnApps.Imports;

    public class ImportReportFacadeService : IImportReportFacadeService
    {
        private readonly IImportReportService importReportService;

        private readonly IPdfService pdfService;

        public ImportReportFacadeService(
            IImportReportService importReportService, IPdfService pdfService)
        {
            this.importReportService = importReportService;
            this.pdfService = pdfService;
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
    }
}
