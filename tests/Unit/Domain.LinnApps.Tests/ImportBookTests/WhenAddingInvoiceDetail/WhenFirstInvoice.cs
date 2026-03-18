namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenAddingInvoiceDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenFirstInvoice
    {
        private ImportBook sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ImportBook()
                       {
                           Id = 1
                       };

            var candidate = new ImportInvoiceDetailCandidate(
                              "1",
                              100) { Currency = TestCurrencies.SwedishKrona };

            this.sut.AddInvoiceDetail(candidate);
        }

        [Test]
        public void ShouldMakeInvoiceDetail()
        {
            this.sut.InvoiceDetails.Should().NotBeNull();
            this.sut.InvoiceDetails.Count.Should().Be(1);
            var invoice = this.sut.InvoiceDetails.First();
            invoice.LineNumber.Should().Be(1);
            invoice.InvoiceNumber.Should().Be("1");
            invoice.InvoiceValue.Should().Be(100);
        }

        [Test]
        public void ShouldUpdateImpbook()
        {
            this.sut.Currency.Should().NotBeNull();
            this.sut.Currency.Should().Be(TestCurrencies.SwedishKrona);
            this.sut.CurrencyCode.Should().Be(TestCurrencies.SwedishKrona.Code);
            this.sut.TotalImportValue.Should().Be(100);
        }
    }
}
