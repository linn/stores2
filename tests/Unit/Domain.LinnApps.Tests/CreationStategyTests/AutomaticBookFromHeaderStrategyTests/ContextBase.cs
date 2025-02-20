namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests.AutomaticBookFromHeaderStrategyTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NUnit.Framework;

    public class ContextBase : ContextBaseForStrategies
    {
        protected AutomaticBookFromHeaderStrategy Sut { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new AutomaticBookFromHeaderStrategy(this.RequisitionRepository, this.RequisitionManager);
        }
    }
}
