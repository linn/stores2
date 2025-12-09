namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCheckingIsReverse 
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var toBeReversed = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.ReturnToSupplier,
                null,
                12345678,
                "RO",
                null,
                null,
                reference: null,
                comments: "Uno reverse",
                quantity: 1,
                fromState: "QC");
            ;
            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.ReturnToSupplier,
                null,
                12345678,
                "RO",
                null,
                null,
                reference: null,
                comments: "Uno reverse",
                quantity: 1,
                isReversalOf: toBeReversed,
                isReverseTrans: "Y");
        }

        [Test]
        public void ShouldBeReverse()
        {
            this.sut.IsReverseTrans().Should().BeTrue();
        }
    }
}
