namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.TestData.CpcNumbers;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenAddingPoImportBook : ContextBase
    {
        [SetUp]
        public void SetUp()
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

            var orderDetail = new ImportOrderDetailCandidate(purchaseOrder, TestCpcNumbers.MaterialCpc);

            var candidate = new ImportCandidate
            {
                Id = 1,
                TransportBillNumber = "12345",
                Supplier = TestSuppliers.AcmeIncorporated,
                Carrier = TestSuppliers.Fedex,
                Currency = TestCurrencies.USDollar,
                BaseCurrency = TestCurrencies.UKPound,
                CreatedBy = TestEmployees.SophlyBard,
                OrderDetailCandidates = new List<ImportOrderDetailCandidate> { orderDetail },
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>
                {
                    new ImportInvoiceDetailCandidate("SPKR-123", 100)
                }
            };

            this.Sut = new ImportClearanceInstruction(this.Master, "Marvin@tnt.com");

            this.Sut.AddImportBook(new ImportBook(candidate), this.AuthNumbers);
        }

        [Test]
        public void ShouldHaveCarrierDetails()
        {
            this.Sut.Carrier.Should().NotBeNull();
            this.Sut.Carrier.Name.Should().Be(TestSuppliers.Fedex.Name);
            this.Sut.TransportBillNumber.Should().Be("12345");
        }

        [Test]
        public void ShouldHaveShippingDetails()
        {
            this.Sut.Supplier.Should().NotBeNull();
            this.Sut.Supplier.Name.Should().Be(TestSuppliers.AcmeIncorporated.Name);
            this.Sut.Carrier.Should().NotBeNull();
            this.Sut.Invoices.Should().Be("SPKR-123");
        }

        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count().Should().Be(1);
            this.Sut.Totals.First().ToString().Should().Be("USD 100.00");
        }

        [Test]
        public void ShouldHaveReturnForRepairSection()
        {
            this.Sut.Sections.Should().NotBeNull();
            this.Sut.Sections.Count.Should().Be(1);
            var section = this.Sut.Sections.First();

            section.Should().NotBeNull();

            section.ReasonForImport.Should().Be("Raw Materials");
            section.CpcNumber.Should().Be("40 00 000");
            section.CpcScheme.Should().BeNull();
            section.IPR.Should().BeFalse();
            section.CDSDeclaration.Should().BeTrue();

            section.Details.Should().NotBeNull();
            section.Details.Should().HaveCount(1);

            var detail = section.Details.First();
            detail.Should().NotBeNull();

            detail.Currency.Code.Should().Be("USD");
            detail.CustomsValue.Should().Be(100);
            detail.Description.Should().Be("Bespoke bass driver for the 119 loudspeaker");
            detail.InvoiceNumber.Should().Be("SPKR-123");
            detail.CountryOfOrigin.Should().Be("NO");
            detail.CustomsValueString().Should().Be("USD 100.00");
        }
    }
}
