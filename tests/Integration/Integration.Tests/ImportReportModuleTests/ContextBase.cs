namespace Linn.Stores2.Integration.Tests.ImportClearanceEmailTests
{
    using System.Net.Http;

    using Linn.Common.Domain;
    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Imports;
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

        protected IImportReportService ImportReportService { get; private set; }

        protected IPdfService PdfService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ImportReportService = Substitute.For<IImportReportService>();
            this.PdfService = Substitute.For<IPdfService>();

            var facadeService = new ImportReportFacadeService(
                this.ImportReportService,
                this.PdfService,
                new ReportReturnResourceBuilder());

            this.Client = TestClient.With<ImportReportModule>(
                services =>
                    {
                        services.AddSingleton<IImportReportFacadeService>(facadeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
