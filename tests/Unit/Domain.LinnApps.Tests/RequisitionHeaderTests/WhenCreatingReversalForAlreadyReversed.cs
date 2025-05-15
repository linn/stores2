namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCreatingReversalForAlreadyReversed
    {
        private RequisitionHeader toReverse;

        private Action action;

        [SetUp]
        public void Setup()
        {
            this.toReverse = new ReqWithReqNumber(
                123456,
                new Employee { Id = 33087 },
                TestFunctionCodes.GistPo,
                null,
                600123,
                "PO",
                null,
                null,
                reference: "MY REF",
                quantity: 500,
                fromState: "QC",
                fromStockPool: "FAIL",
                batchRef: "P600123",
                batchDate: DateTime.UnixEpoch,
                toStockPool: "LINN",
                fromPalletNumber: 666,
                hasBeenReversed: true);

            this.action = () => new RequisitionHeader(
                new Employee { Id = 33087 },
                TestFunctionCodes.GistPo,
                null,
                600123,
                "PO",
                null,
                null,
                isReverseTrans: "Y",
                isReversalOf: this.toReverse);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage(
                    "req 123456 is already reversed!");
        }
    }
}
