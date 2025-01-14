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

    public class StoragePlaceAuditReportFacadeService : IStoragePlaceAuditReportFacadeService
    {
        private readonly IStoragePlaceAuditReportService storagePlaceAuditReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        private readonly IPdfService pdfService;

        private readonly IHtmlTemplateService<StoragePlaceAuditReport> htmlTemplateServiceForStorageAudit;

        public StoragePlaceAuditReportFacadeService(
            IStoragePlaceAuditReportService storagePlaceAuditReportService,
            IReportReturnResourceBuilder resourceBuilder,
            IPdfService pdfService,
            IHtmlTemplateService<StoragePlaceAuditReport> htmlTemplateServiceForStorageAudit)
        {
            this.storagePlaceAuditReportService = storagePlaceAuditReportService;
            this.resourceBuilder = resourceBuilder;
            this.pdfService = pdfService;
            this.htmlTemplateServiceForStorageAudit = htmlTemplateServiceForStorageAudit;
        }

        public IResult<ReportReturnResource> GetStoragePlaceAuditReport(
            IEnumerable<string> locationList,
            string locationRange)
        {
            var result = this.storagePlaceAuditReportService.StoragePlaceAuditReport(locationList, locationRange);
            
            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }

        public async Task<Stream> GetStoragePlaceAuditReportAsPdf(string[] locationList, string locationRange)
        {
            var result = this.storagePlaceAuditReportService.StoragePlaceAuditReport(locationList, locationRange);

            var data = new StoragePlaceAuditReport(result);

            var html = await this.htmlTemplateServiceForStorageAudit.GetHtml(data);

            return await this.pdfService.ConvertHtmlToPdf(html, false);
        }
    }
}
