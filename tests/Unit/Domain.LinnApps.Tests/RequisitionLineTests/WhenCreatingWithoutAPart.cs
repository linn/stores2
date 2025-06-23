namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenCreatingWithoutAPart : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut = new RequisitionLine(1, 1, null, 2, TestTransDefs.LinnDeptToStock);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Requisition line requires a part");
        }
    }
}
