namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenCheckingAuthorisationAndCancelled
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLoan);
            this.sut.Cancel(100, "For Test", new DateTime(2024,1,1));
        }

        [Test]
        public void ShouldNotRequireAuthorisation()
        {
            this.sut.RequiresAuthorisation().Should().BeFalse();
        }
    }
}
