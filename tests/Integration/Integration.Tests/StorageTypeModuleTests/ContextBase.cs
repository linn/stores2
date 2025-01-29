namespace Linn.Stores2.Integration.Tests.StorageTypeModuleTests
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

            var transactionManager = new TransactionManager(this.DbContext);

            var storageTypeRepository
                = new EntityFrameworkRepository<StorageType, string>(this.DbContext.StorageTypes);

            IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource> storageTypeFacadeService
                = new StorageTypeFacadeService(
                    storageTypeRepository,
                    transactionManager,
                    new StorageTypeResourceBuilder());

            this.Client = TestClient.With<StorageTypeModule>(
                services =>
                {
                    services.AddSingleton(storageTypeFacadeService);
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
