namespace Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests
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

        protected IDailyEuReportService DailyEuReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DailyEuReportService = Substitute.For<IDailyEuReportService>();

            IDailyEuReportFacadeService dailyEuReportFacadeService
                = new DailyEuReportsFacadeService(
                    this.DailyEuReportService,
                    new ReportReturnResourceBuilder());

            this.Client = TestClient.With<DailyEuReportsModule>(
                services =>
                {
                    services.AddSingleton(dailyEuReportFacadeService);
                    services.AddHandlers();
                    services.AddRouting();
                });
        }
    }
}
