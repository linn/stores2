namespace Linn.Stores2.Integration.Tests.StockTransactionModuleTests
{
    using System.Net.Http;

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

        protected IStoresTransViewerReportFacadeService StoresTransViewerReportFacadeService { get; private set; }

        protected IStoresTransViewerReportService StoresTransViewerReportService { get; private set; }

        protected IPdfService PdfService { get; private set; }

        protected IHtmlTemplateService<StoresTransactionReport> HtmlTemplateServiceForStoresTransaction { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StoresTransViewerReportService = Substitute.For<IStoresTransViewerReportService>();
            this.HtmlTemplateServiceForStoresTransaction = Substitute.For<IHtmlTemplateService<StoresTransactionReport>>();
            this.PdfService = Substitute.For<IPdfService>();

            this.StoresTransViewerReportFacadeService = new StoresTransViewerReportFacadeService(
                this.StoresTransViewerReportService,
                new ReportReturnResourceBuilder(), 
                this.PdfService,
                this.HtmlTemplateServiceForStoresTransaction);

            this.Client = TestClient.With<StockTransactionModule>(
                services =>
                    {
                        services.AddSingleton(this.StoresTransViewerReportFacadeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
