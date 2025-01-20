namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
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

        protected IRequisitionService DomainService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DbContext = new TestServiceDbContext();

            var requisitionRepository
                = new EntityFrameworkRepository<RequisitionHeader, int>(this.DbContext.RequisitionHeaders);
            this.DomainService = Substitute.For<IRequisitionService>();
            IRequisitionFacadeService
                requisitionService = new RequisitionFacadeService(
                    requisitionRepository,
                    new TransactionManager(this.DbContext),
                    new RequisitionResourceBuilder(),
                    this.DomainService);

            this.Client = TestClient.With<RequisitionModule>(
                services =>
                    {
                        services.AddSingleton(requisitionService);
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
