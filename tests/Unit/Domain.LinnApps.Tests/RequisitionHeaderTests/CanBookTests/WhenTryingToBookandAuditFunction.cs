namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NUnit.Framework;

    public class WhenTryingToBookAndAuditFunction : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.Audit,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book",
                auditLocation: "P123");

            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
