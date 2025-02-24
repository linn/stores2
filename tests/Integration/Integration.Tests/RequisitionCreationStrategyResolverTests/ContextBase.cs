namespace Linn.Stores2.Integration.Tests.RequisitionCreationStrategyResolverTests
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.IoC;

    using Microsoft.Extensions.DependencyInjection;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected ICreationStrategyResolver Sut { get; private set; }

        [SetUp]
        public void SetUp()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddPersistence();
            services.AddServices();
            services.AddLog();
            this.ServiceProvider = services.BuildServiceProvider();

            this.Sut = new RequisitionCreationStrategyResolver(this.ServiceProvider);
        }
    }
}
