namespace Linn.Stores2.Integration.Tests.CountryModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ICountryService CountryService { get; private set; }

        protected IRepository<Country, string> CountryRepository { get; private set; }

        protected TestServiceDbContext DbContext { get; private set; }


        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            this.CountryRepository = new EntityFrameworkRepository<Country, string>(this.DbContext.Countries);

            this.CountryService = new CountryService(this.CountryRepository);

            this.Client = TestClient.With<CountryModule>(
                services =>
                    {
                        services.AddSingleton(this.CountryService);
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
