namespace Linn.Stores2.Integration.Tests.PcasBoardModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources.Pcas;
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

            var pcasBoardRepository = new EntityFrameworkRepository<PcasBoard, string>(this.DbContext.PcasBoards);
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            var transactionManager = new TransactionManager(this.DbContext);

            IAsyncFacadeService<PcasBoard, string, PcasBoardResource, PcasBoardResource, PcasBoardResource> pcasBoardFacadeService
                = new PcasBoardService(
                    pcasBoardRepository,
                    transactionManager,
                    new PcasBoardResourceBuilder(this.AuthorisationService));

            this.Client = TestClient.With<PcasBoardModule>(
                services =>
                {
                    services.AddSingleton(pcasBoardFacadeService);
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
