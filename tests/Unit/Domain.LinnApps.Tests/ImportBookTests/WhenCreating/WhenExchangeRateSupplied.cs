namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenCreating
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenExchangeRateSupplied
    {
        private ImportBook sut;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportCandidate
            {
                CreatedBy = TestEmployees.SophlyBard,
                Supplier = TestSuppliers.TaktAndTon,
                Carrier = TestSuppliers.Fedex,
                BaseCurrency = TestCurrencies.UKPound,
                Currency = TestCurrencies.SwedishKrona,
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>()
                {
                    new ImportInvoiceDetailCandidate("TEST", 100)
                },
                ExchangeRate = new ImportBookExchangeRate
                {
                    BaseCurrency = TestCurrencies.UKPound.Code,
                    ExchangeCurrency = TestCurrencies.SwedishKrona,
                    ExchangeCurrencyCode = TestCurrencies.SwedishKrona.Code,
                    ExchangeRate = 12.34m
                }
            };

            this.sut = new ImportBook(candidate);
        }

        [Test]
        public void ShouldHaveExchangeRate()
        {
            this.sut.ExchangeCurrency.Should().NotBeNull();
            this.sut.ExchangeCurrency.Code.Should().Be(TestCurrencies.SwedishKrona.Code);
        }

        [Test]
        public void ShouldUpdateTotalValue()
        {
            this.sut.TotalImportValue.Should().Be(8.10m);
        }
    }
}
