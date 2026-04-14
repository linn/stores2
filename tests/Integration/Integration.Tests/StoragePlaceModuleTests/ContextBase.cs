namespace Linn.Stores2.Integration.Tests.StoragePlaceModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IStoragePlaceAuditReportFacadeService StoragePlaceAuditReportFacadeService { get; private set; }

        protected IStoragePlaceAuditReportService StoragePlaceAuditReportService { get; private set; }

        protected IPdfService PdfService { get; private set; }

        protected IUserPrivilegeService UserPrivilegeService { get; private set; }

        protected IHtmlTemplateService<StoragePlaceAuditReport> HtmlTemplateServiceForStorageAudit { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StoragePlaceAuditReportService = Substitute.For<IStoragePlaceAuditReportService>();
            this.HtmlTemplateServiceForStorageAudit = Substitute.For<IHtmlTemplateService<StoragePlaceAuditReport>>();
            this.PdfService = Substitute.For<IPdfService>();
            this.UserPrivilegeService = Substitute.For<IUserPrivilegeService>();

            this.StoragePlaceAuditReportFacadeService = new StoragePlaceAuditReportFacadeService(
                this.StoragePlaceAuditReportService,
                new ReportReturnResourceBuilder(),
                this.PdfService,
                this.HtmlTemplateServiceForStorageAudit);

            this.Client = TestClient.With<StoragePlaceModule>(
                services =>
                    {
                        services.AddSingleton(this.StoragePlaceAuditReportFacadeService);
                        services.AddSingleton(this.UserPrivilegeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
