namespace Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources.Pcas;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IReportReturnResourceBuilder ReportReturnResourceBuilder { get; private set; }

        protected IDailyEuReportService DailyEuReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();
            this.ReportingHelper = Substitute.For<IReportingHelper>();
            this.ReportReturnResourceBuilder = new ReportReturnResourceBuilder();
            this.DailyEuReportService = Substitute.For<IDailyEuReportService>();

            var pcasStorageTypeRepository = new PcasStorageTypeRepository(this.DbContext);

            IDailyEuReportFacdeService pcasStorageTypeFacadeService = new DailyEuReportsFacadeService(
                this.DailyEuReportService,
                this.ReportReturnResourceBuilder);

            this.Client = TestClient.With<DailyEuReportsModule>(
                services =>
                    {
                        services.AddSingleton(pcasStorageTypeFacadeService);
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
            this.DbContext.PcasStorageTypes.RemoveAllAndSave(this.DbContext);
        }

    }
}
