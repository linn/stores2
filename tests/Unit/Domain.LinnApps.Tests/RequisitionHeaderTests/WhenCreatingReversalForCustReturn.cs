namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;
    using FluentAssertions;

    public class WhenCreatingReversalForCustReturn
    {
        private RequisitionHeader toReverse;

        private RequisitionHeader sut;

        [SetUp]
        public void Setup()
        {
            this.toReverse = new ReqWithReqNumber(
                123456,
                new Employee { Id = 100 },
                TestFunctionCodes.CustomerReturn,
                null,
                491554,
                "C",
                null,
                null,
                reference: null,
                quantity: 1,
                toState: "STORES",
                toStockPool: "LINN");

            this.sut = new RequisitionHeader(
                new Employee { Id = 7000 },
                TestFunctionCodes.CustomerReturn,
                null,
                491554,
                "C",
                null,
                null,
                isReverseTrans: "Y",
                isReversalOf: this.toReverse);
        }

        [Test]
        public void ShouldMakeReqPreview()
        {
            this.sut.Should().NotBeNull();
        }
    }
}
