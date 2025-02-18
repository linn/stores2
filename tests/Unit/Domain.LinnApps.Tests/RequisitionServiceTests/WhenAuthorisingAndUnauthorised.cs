namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    public class WhenAuthorisingAndUnauthorised : ContextBase
    {
        private RequisitionHeader req;

        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var user = new User
            {
                UserNumber = 33087,
                Privileges = new List<string>()
            };

            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
            this.req.Lines.Add(new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLinnDept));

            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            this.AuthService.HasPermissionFor(
                    this.req.AuthorisePrivilege(), Arg.Any<IEnumerable<string>>())
                .Returns(false);
            this.action = async () => await this.Sut.AuthoriseRequisition(123, user);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<UnauthorisedActionException>();
        }
    }
}
