namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenUpdating
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenAddingExchangeRate
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
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>
                {
                    new ImportInvoiceDetailCandidate("TEST", 100)
                },
                OrderDetailCandidates = new List<ImportOrderDetailCandidate>
                {
                    new ImportOrderDetailCandidate { LineType = "SUNDRY", LineNumber = 1, CurrencyOrderValue = 100 }
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
                CustomsPeriod = new LedgerPeriod { PeriodNumber = 1 },
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

        [Test]
        public void ShouldUpdateOrderDetails()
        {
            var orderDetail = this.sut.OrderDetails.FirstOrDefault();

            orderDetail.Should().NotBeNull();
            orderDetail.CurrencyOrderValue.Should().Be(100);
            orderDetail.OrderValue.Should().Be(8.10m);
        }
    }
}
