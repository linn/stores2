namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCheckingAuthorisationAndRawMaterial : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.BooleanResult = this.Sut.RequiresAuthorisation();
        }

        [Test]
        public void ShouldNotRequireAuthorisation()
        {
            this.BooleanResult.Should().BeFalse();
        }
    }
}
