namespace Linn.Stores2.Facade.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class RequisitionReportFacadeService : IRequisitionReportFacadeService
    {
        private readonly IRequisitionReportService requisitionReportService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        private readonly IPdfService pdfService;

        public RequisitionReportFacadeService(
            IRequisitionReportService requisitionReportService,
            IReportReturnResourceBuilder reportResourceBuilder,
            IPdfService pdfService)
        {
            this.requisitionReportService = requisitionReportService;
            this.reportResourceBuilder = reportResourceBuilder;
            this.pdfService = pdfService;
        }

        public async Task<IResult<ReportReturnResource>> GetRequisitionCostReport(int reqNumber)
        {
            ResultsModel result;

            try
            {
                result = await this.requisitionReportService.GetRequisitionCostReport(reqNumber);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<ReportReturnResource>(exception.Message);
            }

            return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(result));
        }

        public async Task<string> GetRequisitionAsHtml(int reqNumber)
        {
            var htmlResult = await this.requisitionReportService.GetRequisitionAsHtml(reqNumber);

            return htmlResult;
        }

        public async Task<Stream> GetRequisitionAsPdf(int reqNumber)
        {
            var html = await this.requisitionReportService.GetRequisitionAsHtml(reqNumber);

            return await this.pdfService.ConvertHtmlToPdf(html, false);
        }
    }
}
