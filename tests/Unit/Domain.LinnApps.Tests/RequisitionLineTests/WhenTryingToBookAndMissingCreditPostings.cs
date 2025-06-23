namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;

    using Linn.Stores2.TestData.NominalAccounts;

    using NUnit.Framework;

    public class WhenTryingToBookAndMissingCreditPostings : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            this.Sut.AddPosting("C", 2, TestNominalAccounts.FinAssWipUsed);

            this.ProcessResult = this.Sut.CanBookLine();
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.ProcessResult.Success.Should().BeFalse();
            this.ProcessResult.Message.Should().Be("Posting quantities incorrect on line 1.");
        }
    }
}
