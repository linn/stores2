namespace Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests
{
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Service.Modules;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System.Net.Http;

    using NSubstitute;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IDailyEuReportService DailyEuReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

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

        [OneTimeTearDown]
        public void TearDownContext()
        {
            this.DbContext.Dispose();
        }

        [TearDown]
        public void Teardown()
        {
            this.DbContext.DailyEuDespatchReport.RemoveAllAndSave(this.DbContext);
            this.DbContext.DailyEuRsnImportReport.RemoveAllAndSave(this.DbContext);
        }
    }
}

