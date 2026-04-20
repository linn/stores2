namespace Linn.Stores2.Integration.Tests.ImportReportModuleTests
{
    using System.Net.Http;

    using Domain.LinnApps.Imports;

    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Proxy.LinnApps.Services;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    using Resources.Imports;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IReportReturnResourceBuilder ReportReturnResourceBuilder { get; private set; }

        protected IImportReportService ImportReportService { get; private set; }

        protected IStorageTypeService StorageTypeService { get; set; }

        protected IPdfService PdfService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            this.PdfService = Substitute.For<IPdfService>();
            this.ImportReportService = Substitute.For<IImportReportService>();
            this.ReportReturnResourceBuilder = new ReportReturnResourceBuilder();


            IImportReportFacadeService importBookReportFacadeService
                = new ImportReportFacadeService(this.ImportReportService, this.PdfService, this.ReportReturnResourceBuilder);

            this.Client = TestClient.With<ImportReportModule>(services =>
                    {
                        services.AddSingleton(importBookReportFacadeService);
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
            this.DbContext.AccountingCompanies.RemoveAllAndSave(this.DbContext);
            this.DbContext.StorageLocations.RemoveAllAndSave(this.DbContext);
            this.DbContext.StockPools.RemoveAllAndSave(this.DbContext);
        }
    }
}
