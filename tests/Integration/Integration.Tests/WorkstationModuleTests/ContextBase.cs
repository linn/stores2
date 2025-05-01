namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources.Stores;
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

            var workstationRepository
                = new EntityFrameworkRepository<Workstation, string>(this.DbContext.Workstations);

            var employeeRepository
                = new EntityFrameworkRepository<Employee, int>(this.DbContext.Employees);

            var citRepository
                = new EntityFrameworkRepository<Cit, string>(this.DbContext.Cits);

            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> workstationFacadeService
                = new WorkstationFacadeService(
                    workstationRepository,
                    employeeRepository,
                    citRepository,
                    transactionManager,
                    new WorkstationResourceBuilder(new WorkstationElementsResourceBuilder()));

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
