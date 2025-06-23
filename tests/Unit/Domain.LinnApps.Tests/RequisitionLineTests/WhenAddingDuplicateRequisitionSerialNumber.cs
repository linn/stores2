namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NUnit.Framework;

    public class WhenAddingDuplicateRequisitionSerialNumber : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.Sut.AddSerialNumber(1);

            this.action = () => this.Sut.AddSerialNumber(1);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Trying to add duplicate serial number 1 to line 1");
        }
    }
}
