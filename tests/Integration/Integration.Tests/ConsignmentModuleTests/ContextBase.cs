namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Services;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources.Consignments;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IPackingListService PackingListService { get; private set; }

        protected IRepository<Consignment, int> ConsignmentRepository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IUserPrivilegeService UserPrivilegeService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PackingListService = Substitute.For<IPackingListService>();
            this.ConsignmentRepository = Substitute.For<IRepository<Consignment, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.UserPrivilegeService = Substitute.For<IUserPrivilegeService>();

            IPackingListFacadeService packingListFacadeService
                = new PackingListFacadeService(this.PackingListService);

            IAsyncFacadeService<Consignment, int, ConsignmentResource, ConsignmentResource, ConsignmentSearchResource> consignmentFacadeService
                = new ConsignmentFacadeService(
                    this.ConsignmentRepository,
                    this.TransactionManager,
                    new ConsignmentResourceBuilder());

            this.Client = TestClient.With<ConsignmentModule>(
                services =>
                {
                    services.AddSingleton(packingListFacadeService);
                    services.AddSingleton(consignmentFacadeService);
                    services.AddSingleton(this.UserPrivilegeService);
                    services.AddHandlers();
                    services.AddRouting();
                });
        }
    }
}
