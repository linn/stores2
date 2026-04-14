namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookOrderDetailTests.WhenCreating.WhenLineTypePO
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    using NUnit.Framework;

    public class WhenNoPurchaseOrder
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportOrderDetailCandidate
            {
                LineType = "PO"
            };

            this.action = () => _ = new ImportBookOrderDetail(candidate);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "PO order detail has no Purchase Order supplied");
        }
    }
}
