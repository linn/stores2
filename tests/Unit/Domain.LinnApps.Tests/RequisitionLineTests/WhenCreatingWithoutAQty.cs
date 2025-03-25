namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Parts;
    using NUnit.Framework;

    public class WhenCreatingWithoutAQty
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () =>
                {
                    var sut = new RequisitionLine(1, 1, TestParts.Cap003 ,0, new StoresTransactionDefinition());
                };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Requisition line requires a qty");
        }
    }
}
