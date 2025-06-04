namespace Linn.Stores2.Integration.Tests.StoresPalletModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
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

        protected IRepository<StoresPallet, int> StoresPalletRepository { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            var storesPalletRepository = new StoresPalletRepository(this.DbContext);
            var stockPoolRepository = new EntityFrameworkRepository<StockPool, string>(this.DbContext.StockPools);
            var locationTypeRepository = new EntityFrameworkRepository<LocationType, string>(this.DbContext.LocationTypes);
            var storageLocationRepository = new EntityFrameworkRepository<StorageLocation, int>(this.DbContext.StorageLocations);

            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource> storesPalletFacadeService
                = new StoresPalletFacadeService(
                    storesPalletRepository,
                    transactionManager,
                    new StoresPalletResourceBuilder(),
                    stockPoolRepository,
                    locationTypeRepository,
                    storageLocationRepository);

            this.Client = TestClient.With<StoresPalletModule>(
                services =>
                {
                    services.AddSingleton(storesPalletFacadeService);
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
            this.DbContext.StockPools.RemoveAllAndSave(this.DbContext);
            this.DbContext.LocationTypes.RemoveAllAndSave(this.DbContext);
            this.DbContext.StorageLocations.RemoveAllAndSave(this.DbContext);
        }
    }
}
