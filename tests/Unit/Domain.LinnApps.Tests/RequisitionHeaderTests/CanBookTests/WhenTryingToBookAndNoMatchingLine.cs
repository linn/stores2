namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenTryingToBookAndNoMatchingLine : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book");

            this.Result = this.Sut.RequisitionCanBeBooked(2);
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be($"Selected line not found or unexpected result on lines check for req 0.");
        }
    }
}
