namespace Linn.Stores2.Integration.Tests.PalletModuleTests
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

            var palletRepository = new EntityFrameworkRepository<Pallet, int>(this.DbContext.Pallets);
            var stockPoolRepository = new EntityFrameworkRepository<StockPool, string>(this.DbContext.StockPools);
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<Pallet, int, PalletResource, PalletResource, PalletResource> stockPoolFacadeService
                = new PalletFacadeService(
                    palletRepository,
                    transactionManager,
                    new PalletResourceBuilder(),
                    stockPoolRepository);

            this.Client = TestClient.With<PalletModule>(
                services =>
                {
                    services.AddSingleton(stockPoolFacadeService);
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
        }
    }
}

