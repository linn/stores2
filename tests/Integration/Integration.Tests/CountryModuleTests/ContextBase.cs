namespace Linn.Stores2.Integration.Tests.CountryModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
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

            var countryRepository 
                = new EntityFrameworkRepository<Country, string>(this.DbContext.Countries);

            IAsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource> 
                countryService = new CountryService(
                    countryRepository, 
                    new TransactionManager(this.DbContext), 
                    new CountryResourceBuilder());

            this.Client = TestClient.With<CountryModule>(
                services =>
                    {
                        services.AddSingleton(countryService);
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
