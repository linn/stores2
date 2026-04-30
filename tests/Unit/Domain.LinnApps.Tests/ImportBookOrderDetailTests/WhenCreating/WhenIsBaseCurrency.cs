namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookOrderDetailTests.WhenCreating
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    using NUnit.Framework;

    public class WhenIsBaseCurrency
    {
        private ImportBookOrderDetail sut;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportOrderDetailCandidate
            {
                LineType = "SUNDRY",
                Qty = 1,
                OrderDescription = "Super AI",
                CurrencyOrderValue = 100,
                IsBaseCurrency = true
            };

            this.sut = new ImportBookOrderDetail(candidate);
        }

        [Test]
        public void ShouldSetBaseOrderValue()
        {
            this.sut.CurrencyOrderValue.Should().Be(100);
            this.sut.OrderValue.Should().Be(100);
        }
    }
}
