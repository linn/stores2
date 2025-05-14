namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected TestServiceDbContext DbContext { get; private set; }

        protected IRequisitionManager ReqManager { get; private set; }

        protected IRequisitionFactory RequisitionFactory { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        protected IRepository<RequisitionHistory, int> RequisitionHistoryRepository { get; private set; }
        
        protected IQcLabelPrinterService QcLabelPrinterService { get; private set; }
        
        protected IQueryRepository<SundryBookInDetail> SundryBookInDetailRepository { get; private set; }

        protected IDeliveryNoteFacadeService DeliveryNoteFacadeService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();
            var transactionManager = new TransactionManager(this.DbContext);
            var requisitionRepository
                = new EntityFrameworkRepository<RequisitionHeader, int>(this.DbContext.RequisitionHeaders);
            this.ReqManager = Substitute.For<IRequisitionManager>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.RequisitionFactory = Substitute.For<IRequisitionFactory>();
            this.RequisitionHistoryRepository = Substitute.For<IRepository<RequisitionHistory, int>>();
            this.SundryBookInDetailRepository = Substitute.For<IQueryRepository<SundryBookInDetail>>();
            IRequisitionFacadeService requisitionFacadeService = new RequisitionFacadeService(
                requisitionRepository,
                transactionManager,
                new RequisitionResourceBuilder(this.AuthorisationService),
                this.ReqManager,
                this.RequisitionFactory,
                this.RequisitionHistoryRepository);

            IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource,
                StoresFunctionResource> functionCodeService = new StoresFunctionCodeService(
                new EntityFrameworkRepository<StoresFunction, string>(this.DbContext.StoresFunctionCodes),
                transactionManager,
                new StoresFunctionResourceBuilder(this.AuthorisationService));
            IAsyncQueryFacadeService<SundryBookInDetail, SundryBookInDetailResource, SundryBookInDetailResource>
                sundryBookInDetailFacadeService = new SundryBookInDetailFacadeService(
                this.SundryBookInDetailRepository,
                new SundryBookInDetailResourceBuilder());

            this.QcLabelPrinterService = Substitute.For<IQcLabelPrinterService>();
            IRequisitionLabelsFacadeService requisitionLabelsFacadeService = new RequisitionLabelsFacadeService(this.QcLabelPrinterService);

            this.DeliveryNoteFacadeService = Substitute.For<IDeliveryNoteFacadeService>();

            this.Client = TestClient.With<RequisitionModule>(
                services =>
                    {
                        services.AddSingleton(requisitionFacadeService);
                        services.AddSingleton(functionCodeService);
                        services.AddSingleton(requisitionLabelsFacadeService);
                        services.AddSingleton(sundryBookInDetailFacadeService);
                        services.AddSingleton(this.DeliveryNoteFacadeService);
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
