namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Pcas;
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

        protected IStorageTypeService StorageTypeService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();
            var transactionManager = new TransactionManager(this.DbContext);

            var pcasStorageTypeRepository = new PcasStorageTypeRepository(this.DbContext);

            this.StorageTypeService = Substitute.For<IStorageTypeService>();

            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> pcasStorageTypeFacadeService
                = new PcasStorageTypeFacadeService(
                    pcasStorageTypeRepository,
                    transactionManager,
                    new PcasStorageTypeResourceBuilder(),
                    this.StorageTypeService);

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
