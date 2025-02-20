namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenCheckingAuthorisationAndRawMaterial
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.Cap003, 1, TestTransDefs.StockToLoan);
        }

        [Test]
        public void ShouldNotRequireAuthorisation()
        {
            this.sut.RequiresAuthorisation().Should().BeFalse();
        }

        [Test]
        public void ShouldNotHaveAuthorisePrivilege()
        {
            this.sut.AuthorisePrivilege().Should().BeNull();
        }
    }
}
