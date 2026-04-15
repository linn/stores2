namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System.Net.Http;

    using Linn.Stores2.Domain.LinnApps.Consignments.Services;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IPackingListService PackingListService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PackingListService = Substitute.For<IPackingListService>();

            IPackingListFacadeService packingListFacadeService
                = new PackingListFacadeService(this.PackingListService);

            this.Client = TestClient.With<ConsignmentModule>(
                services =>
                {
                    services.AddSingleton(packingListFacadeService);
                    services.AddHandlers();
                    services.AddRouting();
                });
        }
    }
}
