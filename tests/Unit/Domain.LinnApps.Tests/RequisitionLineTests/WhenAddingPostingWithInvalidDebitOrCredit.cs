namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.TestData.NominalAccounts;

    using NUnit.Framework;

    public class WhenAddingPostingWithInvalidDebitOrCredit : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut.AddPosting("A", 1, TestNominalAccounts.TestNomAcc);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Debit or credit for posting should be D or C");
        }
    }
}
