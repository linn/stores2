namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenAddingInvoiceDetail
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenMixingCurrencies
    {
        private Action action;

        private ImportBook sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ImportBook
            {
                Id = 1
            };

            var firstCandidate = new ImportInvoiceDetailCandidate(
                                     "1",
                                     100)
            { Currency = TestCurrencies.SwedishKrona };

            var secondCandidate = new ImportInvoiceDetailCandidate(
                                      "2",
                                      50)
            { Currency = TestCurrencies.USDollar };

            this.sut.AddInvoiceDetail(firstCandidate);

            this.action = () => this.sut.AddInvoiceDetail(secondCandidate);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    $"Invoice detail currency USD does not match import book currency SEK");
        }
    }
}
