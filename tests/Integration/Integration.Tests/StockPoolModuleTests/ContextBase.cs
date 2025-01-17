namespace Linn.Stores2.Integration.Tests.StockPoolModuleTests
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
    using Linn.Stores2.Persistence.LinnApps.Repositories;
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

            var accountingCompanyRepository = new EntityFrameworkRepository<AccountingCompany, string>(this.DbContext.AccountingCompanies);
            var storageLocationRepository = new EntityFrameworkRepository<StorageLocation, int>(this.DbContext.StorageLocations);
            var stockPoolRepository = new StockPoolRepository(this.DbContext);
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource> stockPoolFacadeService 
                = new StockPoolFacadeService(
                    stockPoolRepository, 
                    transactionManager, 
                    new StockPoolResourceBuilder(),
                    storageLocationRepository,
                    accountingCompanyRepository);
            
            this.Client = TestClient.With<StockPoolModule>(
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
           this.DbContext.AccountingCompanies.RemoveAllAndSave(this.DbContext);
           this.DbContext.StorageLocations.RemoveAllAndSave(this.DbContext);
           this.DbContext.StockPools.RemoveAllAndSave(this.DbContext);
        }
    }
}
