namespace Linn.Stores2.Integration.Tests.StoresBudgetModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
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

            var storesBudgetRepository = new StoresBudgetRepository(this.DbContext);
            var transactionManager = new TransactionManager(this.DbContext);
            this.AuthorisationService = Substitute.For<IAuthorisationService>();

            IAsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetResource>
                storesBudgetFacadeService = new StoresBudgetFacadeService(
                    storesBudgetRepository,
                    transactionManager,
                    new StoresBudgetResourceBuilder(this.AuthorisationService));
            
            this.Client = TestClient.With<StoresBudgetModule>(
                services =>
                    {
                        services.AddSingleton(storesBudgetFacadeService);
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
           this.DbContext.StoresBudgets.RemoveAllAndSave(this.DbContext);
        }
    }
}
