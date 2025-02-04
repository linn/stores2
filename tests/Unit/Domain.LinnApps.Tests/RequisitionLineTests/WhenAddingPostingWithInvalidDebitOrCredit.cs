using Linn.Stores2.TestData.NominalAccounts;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenAddingPostingWithInvalidDebitOrCredit
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var sut = new RequisitionLine(1, 1, TestParts.Cap003, 2, TestTransDefs.LinnDeptToStock);
            
            this.action = () =>
            {
                sut.AddPosting("A", 1, TestNominalAccounts.TestNomAcc);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Debit or credit for posting should be D or C");
        }
    }
}
