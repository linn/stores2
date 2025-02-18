namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    public class WhenAuthorising : ContextBase
    {
        private RequisitionHeader req;

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
                .Returns(true);

            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee { Id = 33087 });

            this.Sut.AuthoriseRequisition(123, user);
        }

        [Test]
        public void ShouldBeAuthorised()
        {
            this.req.ReqNumber.Should().Be(123);
            this.req.IsAuthorised().Should().BeTrue();
            this.req.AuthorisedBy.Should().NotBeNull();
            this.req.AuthorisedBy.Id.Should().Be(33087);
            this.req.DateAuthorised.Should().NotBeNull();
        }
    }
}
