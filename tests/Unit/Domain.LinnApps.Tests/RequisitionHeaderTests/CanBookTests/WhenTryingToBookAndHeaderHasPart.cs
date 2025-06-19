namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NUnit.Framework;

    public class WhenTryingToBookAndHeaderHasPart : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.Move,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book",
                quantity: 2,
                part: new Part());

            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.Result.Success.Should().BeTrue();
            this.Result.Message.Should().Be("Part is specified on header");
        }
    }
}
