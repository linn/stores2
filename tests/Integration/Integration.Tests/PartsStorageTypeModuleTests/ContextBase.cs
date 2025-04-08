namespace Linn.Stores2.Integration.Tests.PartsStorageTypeModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
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

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IDatabaseService DatabaseService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            var transactionManager = new TransactionManager(this.DbContext);

            var partRepository = new EntityFrameworkRepository<Part, string>(this.DbContext.Parts);
            var storageTypeRepository = new EntityFrameworkRepository<StorageType, string>(this.DbContext.StorageTypes);
            var partsStorageTypeRepository
                = new PartsStorageTypeRepository(this.DbContext);

            this.DatabaseService = Substitute.For<IDatabaseService>();

            IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> partsStorageTypeFacadeService
                = new PartsStorageTypeFacadeService(
                    partsStorageTypeRepository,
                    transactionManager,
                    new PartsStorageTypeResourceBuilder(),
                    partRepository,
                    storageTypeRepository,
                    this.DatabaseService);

            this.Client = TestClient.With<PartsStorageTypeModule>(
                services =>
                    {
                        services.AddSingleton(partsStorageTypeFacadeService);
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
