namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenUnpickingReqMovesAndMoveDoesntExist : ContextBase
    {
        private Func<Task> action;

        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal("0000001234", "SOME NOM"));

            var line = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept);

            this.req.Lines.Add(line);

            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            this.action = async () => await this.Sut.UnpickRequisitionMove(123, 1, 1, 1, 100, false, new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Line 1 on Req 123 has no move with seq 1"); ;
        }
    }
}
