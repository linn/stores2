namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Resources.Imports;
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

            var exchangeRateRepository = new ImportBookExchangeRateRepository(this.DbContext);
            var currencyRepository = new EntityFrameworkQueryRepository<Currency>(this.DbContext.Currencies);
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService
                = new ImportBookExchangeRateFacadeService(
                    exchangeRateRepository,
                    transactionManager,
                    new ImportBookExchangeRateResourceBuilder(),
                    currencyRepository);

            this.Client = TestClient.With<ImportBookExchangeRateModule>(
                services =>
                    {
                        services.AddSingleton(facadeService);
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
            this.DbContext.ImportBookExchangeRates.RemoveAllAndSave(this.DbContext);
            this.DbContext.Currencies.RemoveAllAndSave(this.DbContext);
            this.DbContext.LedgerPeriods.RemoveAllAndSave(this.DbContext);
        }
    }
}
