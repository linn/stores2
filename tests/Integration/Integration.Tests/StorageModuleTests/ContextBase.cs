using System.Net.Http;
using Linn.Common.Persistence.EntityFramework;
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

namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
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

            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>
                storageSiteService = new StorageSiteService(
                    siteRepository,
                    new TransactionManager(this.DbContext),
                    new StorageSiteResourceBuilder());

            this.Client = TestClient.With<StorageModule>(
                services =>
                {
                    services.AddSingleton(storageSiteService);
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
            this.DbContext.Countries.RemoveAllAndSave(this.DbContext);
        }

    }
}
