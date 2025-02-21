namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExecuting : ContextBase
    {
        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = new StoresFunction("MOVE"),
                                                      ToState = "S1",
                                                      PartNumber = "PART",
                                                      CreatedByUserNumber = 123
                                                  };
            await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldCallManager()
        {
            this.RequisitionManager
                .Received()
                .CheckAndBookRequisitionHeader(Arg.Is<RequisitionHeader>(a => a.ToState == "S1"));
        }
    }
}
