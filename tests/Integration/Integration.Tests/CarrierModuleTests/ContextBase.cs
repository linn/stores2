namespace Linn.Stores2.Integration.Tests.CarrierModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
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

            var countryRepository = new EntityFrameworkRepository<Country, string>(this.DbContext.Countries);
            var carrierRepository = new CarrierRepository(this.DbContext);
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource> carrierService 
                = new CarrierService(
                    carrierRepository, 
                    transactionManager, 
                    new CarrierResourceBuilder(), 
                    countryRepository);
            
            this.Client = TestClient.With<CarrierModule>(
                services =>
                    {
                        services.AddSingleton(carrierService);
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
           this.DbContext.Carriers.RemoveAllAndSave(this.DbContext);
        }
    }
}
