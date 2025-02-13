namespace Linn.Stores2.Integration.Tests.GoodsInModuleTests
{
    using System.Net.Http;

    using Linn.Common.Reporting.Resources.ResourceBuilders;
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

        protected IGoodsInLogReportFacadeService GoodsInLogReportFacadeService { get; private set; }

        protected IGoodsInLogReportService GoodsInLogReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.GoodsInLogReportService = Substitute.For<IGoodsInLogReportService>();
            
            this.GoodsInLogReportFacadeService = new GoodsInLogReportFacadeService(
                this.GoodsInLogReportService,
                new ReportReturnResourceBuilder());

            this.Client = TestClient.With<GoodsInModule>(
                services =>
                    {
                        services.AddSingleton(this.GoodsInLogReportFacadeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
