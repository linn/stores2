namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCreatingGistPoReversal
    {
        private RequisitionHeader toReverse;

        private RequisitionHeader reversal;

        [SetUp]
        public void Setup()
        {
            this.toReverse = new ReqWithReqNumber(
                123456,
                new Employee { Id = 33087 },
                TestFunctionCodes.GistPo,
                null,
                600123,
                null,
                null,
                null,
                reference: "MY REF",
                quantity: 500,
                fromState: "QC",
                fromStockPool: "FAIL",
                batchRef: "P600123",
                batchDate: DateTime.UnixEpoch,
                toStockPool: "LINN",
                fromPalletNumber: 666);

            this.reversal = new RequisitionHeader(
                new Employee { Id = 33087 },
                TestFunctionCodes.GistPo,
                null,
                600123,
                null,
                null,
                null,
                isReverseTrans: "Y",
                isReversalOf: this.toReverse);
        }

        [Test]
        public void ShouldCreateReversalWithCorrectValues()
        {
            this.reversal.OriginalReqNumber.Should().Be(this.toReverse.ReqNumber);

            // negate qty of original
            this.reversal.Quantity.Should().Be(this.toReverse.Quantity * -1);

            // copy following fields from original
            this.reversal.Reference.Should().Be(this.toReverse.Reference);
            this.reversal.FromState.Should().Be(this.toReverse.FromState);
            this.reversal.FromStockPool.Should().Be(this.toReverse.FromStockPool);
            this.reversal.ToStockPool.Should().Be(this.toReverse.ToStockPool);
            this.reversal.FromLocation.Should().Be(this.toReverse.FromLocation);
            this.reversal.FromPalletNumber.Should().Be(this.toReverse.FromPalletNumber);

            // don't copy batch info
            this.reversal.BatchRef.Should().BeNull();
            this.reversal.BatchDate.Should().BeNull();
        }
    }
}
