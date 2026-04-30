namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Linn.Common.Authorisation;
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

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        protected IUserPrivilegeService UserPrivilegeService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.UserPrivilegeService = Substitute.For<IUserPrivilegeService>();

            this.AuthorisationService
                .HasPermissionFor(Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var exchangeRateRepository = new ImportBookExchangeRateRepository(this.DbContext);
            var currencyRepository = new EntityFrameworkQueryRepository<Currency>(this.DbContext.Currencies);
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource> facadeService
                = new ImportBookExchangeRateFacadeService(
                    exchangeRateRepository,
                    transactionManager,
                    new ImportBookExchangeRateResourceBuilder(),
                    currencyRepository,
                    this.AuthorisationService);

            this.Client = TestClient.With<ImportBookExchangeRateModule>(
                services =>
                    {
                        services.AddSingleton(facadeService);
                        services.AddSingleton(this.UserPrivilegeService);
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
