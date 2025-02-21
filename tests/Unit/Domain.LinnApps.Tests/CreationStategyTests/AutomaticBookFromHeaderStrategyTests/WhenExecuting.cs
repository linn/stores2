namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExecuting : ContextBase
    {
        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext.CreatedByUserNumber = 123;
            this.RequisitionCreationContext.Header = new RequisitionHeader(
                    new Employee(),
                    new StoresFunction("MOVE"),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    new Part(),
                    null,
                    null,
                    "S1",
                    null);

            await this.Sut.Apply(this.RequisitionCreationContext);
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
