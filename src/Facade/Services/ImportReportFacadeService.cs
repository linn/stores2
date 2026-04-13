namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Facade.Extensions;
    using Linn.Stores2.Resources.Imports;

    public class ImportReportFacadeService : IImportReportFacadeService
    {
        private readonly IImportReportService importReportService;

        private readonly IPdfService pdfService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        public ImportReportFacadeService(
            IImportReportService importReportService,
            IPdfService pdfService,
            IReportReturnResourceBuilder reportResourceBuilder)
        {
            this.importReportService = importReportService;
            this.pdfService = pdfService;
            this.reportResourceBuilder = reportResourceBuilder;
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

        public async Task<IResult<ReportReturnResource>> GetImportBookReport(ImportBookSearchResource searchResource)
        {
            ResultsModel result;

            try
            {
                result = await this.importReportService.GetImportReport(searchResource.ToExpression());
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<ReportReturnResource>(exception.Message);
            }

            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }
    }
}
