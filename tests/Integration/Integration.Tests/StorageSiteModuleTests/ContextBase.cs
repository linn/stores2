namespace Linn.Stores2.Integration.Tests.StorageSiteModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.IoC;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IRepository<StorageSite, string> Repository { get; private set; }

        protected ITransactionManager TransactionManager { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.Repository = Substitute.For<IRepository<StorageSite, string>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service
                = new StorageSiteFacadeService(
                this.Repository,
                this.TransactionManager,
                new StorageSiteResourceBuilder());

            this.Client = TestClient.With<StorageSiteModule>(
                services =>
                    {
                        services.AddSingleton(service);
                        services.AddHandlers();
                        services.AddRouting();
                    });
        }
    }
}
