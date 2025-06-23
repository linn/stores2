namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
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

            var workstationRepository
                = new EntityFrameworkRepository<Workstation, string>(this.DbContext.Workstations);

            var employeeRepository
                = new EntityFrameworkRepository<Employee, int>(this.DbContext.Employees);

            var citRepository
                = new EntityFrameworkRepository<Cit, string>(this.DbContext.Cits);

            var storageLocationRepository
                = new EntityFrameworkRepository<StorageLocation, int>(this.DbContext.StorageLocations);

            var palletRepository
                = new EntityFrameworkRepository<StoresPallet, int>(this.DbContext.StoresPallets);

            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> workstationFacadeService
                = new WorkstationFacadeService(
                    workstationRepository,
                    employeeRepository,
                    citRepository,
                    storageLocationRepository,
                    palletRepository,
                    this.AuthorisationService,
                    transactionManager,
                    new WorkstationResourceBuilder(new WorkstationElementsResourceBuilder(), this.AuthorisationService));

            this.Client = TestClient.With<WorkstationModule>(
                services =>
                {
                    services.AddSingleton(workstationFacadeService);
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
