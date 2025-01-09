namespace Linn.Stores2.Integration.Tests.StoragePlaceModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }
        
        protected TestServiceDbContext DbContext { get; private set; }

        protected IStoragePlaceAuditReportFacadeService StoragePlaceAuditReportFacadeService { get; private set; }

        protected IStoragePlaceAuditReportService StoragePlaceAuditReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StoragePlaceAuditReportService = Substitute.For<IStoragePlaceAuditReportService>();

            this.StoragePlaceAuditReportFacadeService = new StoragePlaceAuditReportFacadeService(
                this.StoragePlaceAuditReportService,
                new ReportReturnResourceBuilder());

            this.Client = TestClient.With<StoragePlaceModule>(
                services =>
                    {
                        services.AddSingleton(this.StoragePlaceAuditReportFacadeService);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
