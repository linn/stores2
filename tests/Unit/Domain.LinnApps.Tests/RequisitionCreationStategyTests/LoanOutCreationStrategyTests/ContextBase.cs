namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStategyTests.LoanOutCreationStrategyTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStategyTests;

    using NUnit.Framework;

    public class ContextBase : ContextBaseForStrategies
    {
        protected LoanOutCreationStrategy Sut { get; set; }

        protected RequisitionHeader Result { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new LoanOutCreationStrategy(
                this.RequisitionManager);
        }
    }
}
