namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;

    using NUnit.Framework;

    public class WhenTryingToBookAndMissingDebitPostings
    {
        private RequisitionLine sut;

        private StoresTransactionDefinition trans;

        [SetUp]
        public void SetUp()
        {
            this.trans = new StoresTransactionDefinition
            {
                TransactionCode = "LDSTI",
                Description = "Onto Transaction",
                OntoTransactions = "Y"
            };

            this.sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, this.trans);

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
