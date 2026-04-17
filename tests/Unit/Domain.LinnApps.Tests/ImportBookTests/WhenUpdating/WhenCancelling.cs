namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenUpdating
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

    public class WhenCancelling
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
                }
            };

            this.sut = new ImportBook(candidate);

            var update = new ImportUpdate
            {
                CancelledBy = TestEmployees.SophlyBard,
                CancelledReason = "Malicious",
            };

            this.sut.Update(update);
        }

        [Test]
        public void ShouldBeCancelled()
        {
            this.sut.IsCancelled.Should().BeTrue();
            this.sut.CancelledBy.Id.Should().Be(TestEmployees.SophlyBard.Id);
            this.sut.CancelledReason.Should().Be("Malicious");
            this.sut.DateCancelled.Should().HaveValue();
        }
    }
}
