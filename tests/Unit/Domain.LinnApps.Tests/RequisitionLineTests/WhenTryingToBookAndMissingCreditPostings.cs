namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;
    using Linn.Stores2.TestData.NominalAccounts;

    public class WhenTryingToBookAndMissingCreditPostings
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, TestTransDefs.LinnDeptToStock);

            this.sut.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            this.sut.AddPosting("C", 2, TestNominalAccounts.FinAssWipUsed);
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook().Should().BeFalse();
        }
    }
}
