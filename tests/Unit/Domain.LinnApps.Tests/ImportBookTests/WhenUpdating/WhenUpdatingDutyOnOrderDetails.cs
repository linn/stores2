namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenUpdating
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenUpdatingDutyOnOrderDetails
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
                OrderDetailCandidates = new List<ImportOrderDetailCandidate>()
                {
                    new ImportOrderDetailCandidate
                    {
                        LineType = "SUNDRY",
                        Qty = 1
                    }
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
                Currency = TestCurrencies.SwedishKrona,
                OrderDetailCandidates = new List<ImportOrderDetailCandidate>()
                {
                    new ImportOrderDetailCandidate
                    {
                        LineType = "SUNDRY",
                        LineNumber = 1,
                        Qty = 1,
                        DutyValue = 13.24m
                    }
                }
            };

            this.sut.Update(update);
        }

        [Test]
        public void ShouldUpdateOrderDetailDuty()
        {
            this.sut.OrderDetails.Should().NotBeNull();
            this.sut.OrderDetails.Should().HaveCount(1);
            var orderDetail = this.sut.OrderDetails.First();
            orderDetail.DutyValue.Should().Be(13.24m);
        }

        [Test]
        public void ShouldUpdateHeaderDuty()
        {
            this.sut.LinnDuty.Should().Be(13.24m);
        }
    }
}
