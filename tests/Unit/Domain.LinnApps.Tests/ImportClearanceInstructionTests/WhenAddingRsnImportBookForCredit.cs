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

    public class WhenAddingRsnImportBookForCredit : ContextBase
    {
        private ImportBook importBook;

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
                    ReasonCode = "RFC",
                    ReasonCategory = "Credit"
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

            var orderDetail = new ImportOrderDetailCandidate(rsn, TestCpcNumbers.BRGCpc);

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

            this.importBook = new ImportBook(candidate);
            this.Sut.AddImportBook(this.importBook, this.AuthNumbers);
        }

        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count().Should().Be(1);
            this.Sut.BRGTotals.First().ToString().Should().Be("SEK 100.00");
            this.Sut.HasIPRAndBRG.Should().BeFalse();
            this.Sut.IPRTotals.Should().BeEmpty();
            this.Sut.ValuePrompt("BRG").Should().Be("Value:");
        }

        [Test]
        public void ShouldHaveReturnForRepairSection()
        {
            this.Sut.Sections.Should().NotBeNull();
            this.Sut.Sections.Count.Should().Be(1);
            var section = this.Sut.Sections.FirstOrDefault();

            section.Should().NotBeNull();

            section.ReasonForImport.Should().Be("Return for Credit");
            section.CpcNumber.Should().Be("61 10 F05 - BRG");
            section.CpcScheme.Should().Be("BRG");
            section.IPR.Should().BeFalse();
            section.Declaration.Should().StartWith("NIRU / RGR / 336 / 0325");

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
