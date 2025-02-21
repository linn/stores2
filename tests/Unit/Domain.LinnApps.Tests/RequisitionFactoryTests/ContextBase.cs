namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionFactoryTests
{
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRequisitionFactory Sut { get; set; }

        protected IRepository<StoresFunction, string> StoresFunctionRepository { get; set; }

        protected ICreationStrategyResolver CreationStrategyResolver { get; set; }

        protected Employee Employee { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.StoresFunctionRepository = Substitute.For<IRepository<StoresFunction, string>>();
            this.CreationStrategyResolver = Substitute.For<ICreationStrategyResolver>();

            this.Sut = new RequisitionFactory(
                this.CreationStrategyResolver,
                this.StoresFunctionRepository);
        }
    }
}
