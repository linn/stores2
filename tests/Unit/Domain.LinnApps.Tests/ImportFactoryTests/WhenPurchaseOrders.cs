namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.TestData.CpcNumbers;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPurchaseOrders : ContextBase
    {
        private ImportCandidate result;

        [SetUp]
        public async Task SetUp()
        {
            var purchaseOrder = new PurchaseOrder
            {
                OrderNumber = 12,
                Supplier = TestSuppliers.SeasFabrikker,
                Details = new List<PurchaseOrderDetail>
                {
                    new PurchaseOrderDetail
                    {
                        SalesArticle = TestSalesArticles.Spkr105,
                        SuppliersDesignation = "Bespoke bass driver for the 119 loudspeaker"
                    }
                }
            };

            this.CurrencyRepository.FindByAsync(Arg.Any<Expression<Func<Currency, bool>>>()).Returns(TestCurrencies.UKPound);

            this.ImportBookCpcNumberRepository.FindByAsync(Arg.Any<Expression<Func<ImportBookCpcNumber, bool>>>())
                .Returns(TestCpcNumbers.MaterialCpc);

            this.PurchaseOrderRepository.FindByIdAsync(12).Returns(purchaseOrder);
            this.result = await this.Sut.CreateImportBook(null, new List<int> { 12 },  null, new Employee());
        }

        [Test]
        public void ShouldDefaultFields()
        {
            this.result.BaseCurrency.Should().Be(TestCurrencies.UKPound);
        }

        [Test]
        public void ShouldMakeCorrectOrderCandidates()
        {
            this.result.OrderDetailCandidates.Should().NotBeNull();
            this.result.OrderDetailCandidates.Count.Should().Be(1);
            var candidate = this.result.OrderDetailCandidates.First();
            candidate.LineType.Should().Be("PO");
            candidate.PurchaseOrder.Should().NotBeNull();
            candidate.OrderDescription.Should().Be("Bespoke bass driver for the 119 loudspeaker");
            candidate.TariffCode.Should().Be(TestSalesArticles.Spkr105.Tariff.TariffCode);
            candidate.CountryOfOrigin.Should().Be(TestSalesArticles.Spkr105.CountryOfOrigin);
            candidate.CpcNumber.Should().NotBeNull();
            candidate.CpcNumber.CpcNumber.Should().Be(TestCpcNumbers.MaterialCpc.CpcNumber);
        }

        [Test]
        public void ShouldNotMakeInvoiceCandidates()
        {
            this.result.InvoiceDetailCandidates.Should().NotBeNull();
            this.result.InvoiceDetailCandidates.Count.Should().Be(0);
        }
    }
}
