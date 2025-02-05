namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;

    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using Linn.Stores2.TestData.NominalAccounts;

    public class WhenTryingToBookTransactionThatDoesntRequireMoves
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, TestTransDefs.SuppToMatVarTrans);

            this.sut.AddPosting("D", 2, TestNominalAccounts.AssetsRawMat);
            this.sut.AddPosting("C", 2, TestNominalAccounts.FinAssWipUsed);
        }

        [Test]
        public void ShouldBeOkToBook()
        {
            this.sut.OkToBook().Should().BeTrue();
        }
    }
}
