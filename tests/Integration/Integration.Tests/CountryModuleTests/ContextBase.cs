namespace Linn.Stores2.Integration.Tests.CountryModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ICountryFacadeService CountryFacadeServiceService { get; private set; }

        protected IRepository<Country, string> CountryRepository { get; private set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected Country GreatBritain { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();
            this.GreatBritain = new Country("GB", "Starmer's Britain");
            this.DbContext.Countries.Add(this.GreatBritain);
            this.DbContext.SaveChanges();

            this.CountryRepository = new CountryRepository(this.DbContext.Countries);

            this.CountryFacadeServiceService = new CountryFacadeService(this.CountryRepository);

            this.Client = TestClient.With<CountryModule>(
                services =>
                    {
                        services.AddSingleton(this.CountryFacadeServiceService);
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
            this.DbContext.Countries.Remove(this.GreatBritain);
            this.DbContext.SaveChanges();
        }
    }
}
