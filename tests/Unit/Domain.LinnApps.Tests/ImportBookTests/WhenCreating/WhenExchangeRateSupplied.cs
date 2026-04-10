namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenCreating
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
                }
            };

            this.sut = new ImportBook(candidate);

            var update = new ImportUpdate
            {
                ExchangeRate = new ImportBookExchangeRate
                {
                    BaseCurrency = TestCurrencies.UKPound.Code,
                    ExchangeCurrency = TestCurrencies.SwedishKrona,
                    ExchangeCurrencyCode = TestCurrencies.SwedishKrona.Code,
                    ExchangeRate = 12.34m
                },
                Period = new LedgerPeriod { PeriodNumber = 1 },
                Currency = TestCurrencies.SwedishKrona
            };

            this.sut.Update(update);
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
