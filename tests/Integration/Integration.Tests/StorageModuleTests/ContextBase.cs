namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Net.Http;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Modules;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            var siteRepository = new StorageSiteRepository(this.DbContext);
            var locationRepository = new StorageLocationRepository(this.DbContext);
            var accountingCompanyRepository = new EntityFrameworkRepository<AccountingCompany, string>(this.DbContext.AccountingCompanies);
            var storageSiteRepository = new StorageSiteRepository(this.DbContext);

            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>
                storageSiteService = new StorageSiteService(
                    siteRepository,
                    new TransactionManager(this.DbContext),
                    new StorageSiteResourceBuilder());

            var databaseSequenceService = new TestDatabaseSequenceService();

            IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource>
                storageLocationService = new StorageLocationService(
                    locationRepository,
                    new TransactionManager(this.DbContext),
                    new StorageLocationResourceBuilder(),
                    databaseSequenceService, accountingCompanyRepository, storageSiteRepository);

            this.Client = TestClient.With<StorageModule>(
                services =>
                {
                    services.AddSingleton(storageSiteService);
                    services.AddSingleton(storageLocationService);
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
            this.DbContext.StorageSites.RemoveAllAndSave(this.DbContext);
            this.DbContext.StorageLocations.RemoveAllAndSave(this.DbContext);
        }
    }
}
