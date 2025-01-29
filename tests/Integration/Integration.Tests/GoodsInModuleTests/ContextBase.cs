namespace Linn.Stores2.Integration.Tests.GoodsInModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources.GoodsIn;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

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
            
            var goodsInLogRepository = new GoodsInLogEntryRepository(this.DbContext);

            IAsyncFacadeService<GoodsInLogEntry, int, GoodsInLogEntryResource, GoodsInLogEntryResource, GoodsInLogEntrySearchResource> goodsInLogFacadeService
                = new GoodsInLogFacadeService(
                    goodsInLogRepository,
                    new TransactionManager(this.DbContext),
                    new GoodsInLogEntryResourceBuilder());

            this.Client = TestClient.With<GoodsInModule>(
                services =>
                    {
                        services.AddSingleton(goodsInLogFacadeService);
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
            this.DbContext.GoodsInLogEntries.RemoveAllAndSave(this.DbContext);
        }
    }
}
