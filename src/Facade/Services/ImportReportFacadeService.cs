namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Pdf;
    using Linn.Stores2.Domain.LinnApps.Imports;

    public class ImportReportFacadeService : IImportReportFacadeService
    {
        private readonly IImportReportService importReportService;

        private readonly IPdfService pdfService;

        public ImportReportFacadeService(IImportReportService importReportService, IPdfService pdfService)
        {
            this.importReportService = importReportService;
            this.pdfService = pdfService;
        }

        public async Task<Stream> GetClearanceInstructionAsPdf(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var html = await this.importReportService.GetClearanceInstructionAsHtml(impbookIds, toEmailAddress);

            return await this.pdfService.ConvertHtmlToPdf(html, false);
        }

        public async Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var htmlResult = await this.importReportService.GetClearanceInstructionAsHtml(impbookIds, toEmailAddress);

            return htmlResult;
        }
    }
}
