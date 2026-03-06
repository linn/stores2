namespace Linn.Stores2.Integration.Tests.ImportBookTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
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

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            this.AuthorisationService = Substitute.For<IAuthorisationService>();

            var importBookRepository = new ImportBookRepository(this.DbContext);
            var employeeRepository
                = new EntityFrameworkRepository<Employee, int>(this.DbContext.Employees);
            var supplierRepository
                = new EntityFrameworkQueryRepository<Supplier>(this.DbContext.Suppliers);

            var transactionManager = new TransactionManager(this.DbContext);
            var databaseSequenceService = new TestDatabaseSequenceService();

            IAsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource> importBookService
                = new ImportBookFacadeService(
                    importBookRepository,
                    databaseSequenceService,
                    employeeRepository,
                    supplierRepository,
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
    }
}
