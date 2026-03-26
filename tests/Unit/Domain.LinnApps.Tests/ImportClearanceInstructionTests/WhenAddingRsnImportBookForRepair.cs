namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.CpcNumbers;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.SalesOutlets;
    using Linn.Stores2.TestData.Suppliers;
    using NUnit.Framework;

    public class WhenAddingRsnImportBookForRepair : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var rsn = new Rsn
            {
                RsnNumber = 1234,
                SalesOutlet = TestSalesOutlets.TonlagetHifi,
                AccountId = TestSalesOutlets.TonlagetHifi.AccountId,
                OutletNumber = TestSalesOutlets.TonlagetHifi.OutletNumber,
                Quantity = 1,
                Ipr = "Y",
                SalesArticle = TestSalesArticles.Akiva,
                AllegedReason = new RsnReturnReason
                {
                    ReasonCode = "RFR",
                    ReasonCategory = "Repair"
                },
                ImportBookOrderDetails = new List<ImportBookOrderDetail>(),
                ExportReturnDetails = new List<ExportReturnDetail>
                {
                    new ExportReturnDetail
                    {
                        CustomsValue = 100,
                        ExportReturn = new ExportReturn
                        {
                            Currency = TestCurrencies.SwedishKrona
                        }
                    }
                }
            };

            var orderDetail = new ImportOrderDetailCandidate(rsn, TestCpcNumbers.IPRCpc);

            var candidate = new ImportCandidate
            {
                Id = 1,
                TransportBillNumber = "12345",
                Supplier = TestSuppliers.TaktAndTon,
                Carrier = TestSuppliers.Fedex,
                Currency = TestCurrencies.SwedishKrona,
                BaseCurrency = TestCurrencies.UKPound,
                CreatedBy = TestEmployees.SophlyBard,
                OrderDetailCandidates = new List<ImportOrderDetailCandidate> { orderDetail },
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>
                {
                    new ImportInvoiceDetailCandidate(rsn)
                }
            };

            this.Sut = new ImportClearanceInstruction(this.Master, "Marvin@tnt.com");

            this.Sut.AddImportBook(new ImportBook(candidate));
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
            this.Sut.Supplier.Name.Should().Be(TestSuppliers.TaktAndTon.Name);
            this.Sut.Carrier.Should().NotBeNull();
            this.Sut.Invoices.Should().Be("1234");
        }


        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count().Should().Be(1);
            this.Sut.Totals.First().ToString().Should().Be("SEK 100.00");
        }

        [Test]
        public void ShouldHaveReturnForRepairSection()
        {
            this.Sut.Sections.Should().NotBeNull();
            this.Sut.Sections.Count.Should().Be(1);
            var section = this.Sut.Sections.FirstOrDefault();

            section.Should().NotBeNull();

            section.ReasonForImport.Should().Be("Return for Repair");
            section.CpcNumber.Should().Be("51 00 000");
            section.CpcScheme.Should().Be("IPR");
            section.IPR.Should().BeTrue();

            section.Details.Should().NotBeNull();
            section.Details.Should().HaveCount(1);

            var detail = section.Details.FirstOrDefault();
            detail.Should().NotBeNull();

            detail.Currency.Code.Should().Be("SEK");
            detail.CustomsValue.Should().Be(100);
            detail.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail.InvoiceNumber.Should().Be("1234");
            detail.CountryOfOrigin.Should().Be("JP");
            detail.CustomsValueString().Should().Be("SEK 100.00");
        }
    }
}
