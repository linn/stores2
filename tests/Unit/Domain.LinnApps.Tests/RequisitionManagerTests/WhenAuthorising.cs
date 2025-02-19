namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorising : ContextBase
    {
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
                new Nominal());
            this.req.Lines.Add(new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLinnDept));

            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            this.AuthService.HasPermissionFor(
                    this.req.AuthorisePrivilege(), Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var employee = new Employee { Id = 33087 };

            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee { Id = 33087 });

            this.Sut.AuthoriseRequisition(123, employee.Id, new List<string>());
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
