namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenAddingDuplicateRequisitionSerialNumber
    {
        private RequisitionLine sut;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(
                123,
                1,
                new Part(),
                10,
                new StoresTransactionDefinition());

            this.sut.AddSerialNumber(1);

            this.action = () => this.sut.AddSerialNumber(1);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Trying to add duplicate serial number 1 to line 1");
        }
    }
}
