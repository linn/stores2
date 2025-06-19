namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBookTransactionThatDoesNotRequireMoves : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, TestTransDefs.SuppToMatVarTrans);

            this.Sut.AddPosting("D", 2, TestNominalAccounts.AssetsRawMat);
            this.Sut.AddPosting("C", 2, TestNominalAccounts.FinAssWipUsed);

            this.ProcessResult = this.Sut.CanBookLine();
        }

        [Test]
        public void ShouldBeOkToBook()
        {
            this.ProcessResult.Success.Should().BeTrue();
        }
    }
}
