namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyCancelled
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "Goodbye Reqs");
            this.sut.Cancel("Test Porpoises", new Employee());
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.sut.RequisitionIsBookable(null).Should().BeFalse();
        }
    }
}
