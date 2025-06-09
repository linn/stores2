namespace Linn.Stores2.Integration.Tests.RequisitionReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Pdf;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
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
        
        protected IRequisitionReportFacadeService RequisitionReportFacadeService { get; private set; }

        protected IRequisitionReportService RequisitionReportService { get; private set; }

        protected IPdfService PdfService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.RequisitionReportService = Substitute.For<IRequisitionReportService>();
            this.PdfService = Substitute.For<IPdfService>();

            this.RequisitionReportFacadeService = new RequisitionReportFacadeService(
                this.RequisitionReportService,
                new ReportReturnResourceBuilder(),
                this.PdfService);

            this.Client = TestClient.With<RequisitionReportModule>(
                services =>
                    {
                        services.AddSingleton(this.RequisitionReportFacadeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
