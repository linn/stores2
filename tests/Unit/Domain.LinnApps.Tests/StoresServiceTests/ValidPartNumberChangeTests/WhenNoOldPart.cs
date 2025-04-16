namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenNoOldPart : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.NewPart = new Part { PartNumber = "P1", AccountingCompanyCode = "AC2" };

            this.Result = this.Sut.ValidPartNumberChange(null, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Part number change requires old part");
        }
    }
}
