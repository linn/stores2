namespace Linn.Stores2.Integration.Tests.WorkStationModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources.Stores;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            this.AuthorisationService = Substitute.For<IAuthorisationService>();

            var transactionManager = new TransactionManager(this.DbContext);

            var workStationRepository
                = new EntityFrameworkRepository<WorkStation, string>(this.DbContext.WorkStations);

            var employeeRepository
                = new EntityFrameworkRepository<Employee, int>(this.DbContext.Employees);

            var citRepository
                = new EntityFrameworkRepository<Cit, string>(this.DbContext.Cits);

            var storageLocationRepository
                = new EntityFrameworkRepository<StorageLocation, int>(this.DbContext.StorageLocations);

            var palletRepository
                = new EntityFrameworkRepository<StoresPallet, int>(this.DbContext.StoresPallets);

            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> workStationFacadeService
                = new WorkStationFacadeService(
                    workStationRepository,
                    employeeRepository,
                    citRepository,
                    storageLocationRepository,
                    palletRepository,
                    this.AuthorisationService,
                    transactionManager,
                    new WorkStationResourceBuilder(new WorkStationElementsResourceBuilder(), this.AuthorisationService));

            this.Client = TestClient.With<WorkStationModule>(
                services =>
                {
                    services.AddSingleton(workStationFacadeService);
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
            this.DbContext.Cits.RemoveAllAndSave(this.DbContext);
        }
    }
}
