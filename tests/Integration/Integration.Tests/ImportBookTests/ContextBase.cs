namespace Linn.Stores2.Integration.Tests.ImportBookTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Persistence.LinnApps.Repositories;
    using Linn.Stores2.Service.Modules;
    using Linn.Stores2.TestData.Countries;
    using Linn.Stores2.TestData.Currencies;

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

            var importBookRepository = new ImportBookRepository(this.DbContext);
            var employeeRepository
                = new EntityFrameworkRepository<Employee, int>(this.DbContext.Employees);
            var supplierRepository
                = new EntityFrameworkQueryRepository<Supplier>(this.DbContext.Suppliers);
            var currencyRepository
                = new EntityFrameworkQueryRepository<Currency>(this.DbContext.Currencies);
            var rsnRepository = new EntityFrameworkQueryRepository<Rsn>(this.DbContext.Rsns);
            var countryRepository
                = new EntityFrameworkRepository<Country, string>(this.DbContext.Countries);

            var importFactory = new ImportFactory(
                supplierRepository,
                currencyRepository,
                rsnRepository);

            var transactionManager = new TransactionManager(this.DbContext);
            var databaseSequenceService = new TestDatabaseSequenceService();

            IImportBookFacadeService importBookService
                = new ImportBookFacadeService(
                    importBookRepository,
                    databaseSequenceService,
                    employeeRepository,
                    supplierRepository,
                    currencyRepository,
                    rsnRepository,
                    countryRepository,
                    importFactory,
                    transactionManager,
                    this.AuthorisationService,
                    new ImportBookResourceBuilder(
                        new ImportBookPostEntryResourceBuilder(),
                        new ImportBookOrderDetailResourceBuilder(),
                        new ImportBookInvoiceDetailResourceBuilder(),
                        this.AuthorisationService));

            this.Client = TestClient.With<ImportBookModule>(
                services =>
                    {
                        services.AddSingleton(importBookService);
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
            this.DbContext.ImportBooks.RemoveAllAndSave(this.DbContext);
            this.DbContext.Employees.RemoveAllAndSave(this.DbContext);
        }

        public void SetupCurrencies()
        {
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.UKPound);
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.SwedishKrona);
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.USDollar);
        }

        public void SetupCountries()
        {
            this.DbContext.Countries.AddAndSave(this.DbContext, TestCountries.China);
            this.DbContext.Countries.AddAndSave(this.DbContext, TestCountries.UnitedStates);
            this.DbContext.Countries.AddAndSave(this.DbContext, TestCountries.Japan);
            this.DbContext.Countries.AddAndSave(this.DbContext, TestCountries.Norway);
        }
    }
}
