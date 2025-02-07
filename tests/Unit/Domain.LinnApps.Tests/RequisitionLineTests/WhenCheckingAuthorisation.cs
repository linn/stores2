﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using FluentAssertions;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    public class WhenCheckingAuthorisation
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLoan);
        }

        [Test]
        public void ShouldRequireAuthorisation()
        {
            this.sut.RequiresAuthorisation().Should().BeTrue();
        }
    }
}
