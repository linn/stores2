namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Net.Http;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.Common;
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

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            var transactionManager = new TransactionManager(this.DbContext);

            var pcaStorageTypeRepository = new PcasStorageTypeRepository(this.DbContext);

            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> pcasStorageTypeFacadeService
                = new PcasStorageTypeFacadeService(
                    pcaStorageTypeRepository,
                    transactionManager,
                    new PcasStorageTypeResourceBuilder());

            this.Client = TestClient.With<PcasStorageTypeModule>(
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
