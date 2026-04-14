namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using System.Collections.Generic;
    using System.Linq;

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

    public class WhenAddingBothIPRReturnForRepairAndUpgrade : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var iprRsn = new Rsn
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

            var orderDetail = new ImportOrderDetailCandidate(iprRsn, TestCpcNumbers.IPRCpc);

            var brgRsn = new Rsn
            {
                RsnNumber = 2345,
                SalesOutlet = TestSalesOutlets.TonlagetHifi,
                AccountId = TestSalesOutlets.TonlagetHifi.AccountId,
                OutletNumber = TestSalesOutlets.TonlagetHifi.OutletNumber,
                Quantity = 1,
                Ipr = "N",
                SalesArticle = TestSalesArticles.Akiva,
                AllegedReason = new RsnReturnReason
                {
                    ReasonCode = "RFU",
                    ReasonCategory = "Upgrade"
                },
                ImportBookOrderDetails = new List<ImportBookOrderDetail>(),
                ExportReturnDetails = new List<ExportReturnDetail>(),
                RsnReturns = new List<RsnReturnInformation>
                {
                    new RsnReturnInformation
                    {
                        CustomsValue = 150,
                        Currency = TestCurrencies.SwedishKrona
                    }
                }
            };

            var brgDetail = new ImportOrderDetailCandidate(brgRsn, TestCpcNumbers.IPRCpc);

            var candidate = new ImportCandidate
            {
                Id = 1,
                TransportBillNumber = "12345",
                Supplier = TestSuppliers.TaktAndTon,
                Carrier = TestSuppliers.Fedex,
                Currency = TestCurrencies.SwedishKrona,
                BaseCurrency = TestCurrencies.UKPound,
                CreatedBy = TestEmployees.SophlyBard,
                OrderDetailCandidates = new List<ImportOrderDetailCandidate> { orderDetail, brgDetail },
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>
                {
                    new ImportInvoiceDetailCandidate(iprRsn),
                    new ImportInvoiceDetailCandidate(brgRsn)
                }
            };

            this.Sut = new ImportClearanceInstruction(this.Master, "Marvin@tnt.com");

            this.Sut.AddImportBook(new ImportBook(candidate), this.AuthNumbers);
        }

        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count.Should().Be(1);
            this.Sut.HasIPRAndBRG.Should().BeFalse();
            this.Sut.IPRTotals.First().ToString().Should().Be("SEK 250.00");
            this.Sut.BRGTotals.Should().BeEmpty();
            this.Sut.ValuePrompt("IPR").Should().Be("Value:");
        }

        [Test]
        public void ShouldHaveIPRSection()
        {
            this.Sut.Sections.Should().NotBeNull();
            this.Sut.Sections.Count.Should().Be(1);

            var section1 = this.Sut.Sections.First();
            section1.Should().NotBeNull();

            section1.ReasonForImport.Should().Be("Return for Repair / Upgrade");
            section1.CpcNumber.Should().Be("51 00 000");
            section1.CpcScheme.Should().Be("IPR");
            section1.IPR.Should().BeTrue();
            section1.Declaration.Should().StartWith("CDS IPR Reference - GBIPO3830942440020210510102146");
            section1.Details.Should().NotBeNull();
            section1.Details.Should().HaveCount(2);

            var detail1 = section1.Details.First();
            detail1.Should().NotBeNull();

            detail1.Currency.Code.Should().Be("SEK");
            detail1.CustomsValue.Should().Be(100);
            detail1.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail1.InvoiceNumber.Should().Be("1234");
            detail1.CountryOfOrigin.Should().Be("JP");
            detail1.CustomsValueString().Should().Be("SEK 100.00");

            var detail2 = section1.Details.Last();
            detail2.Should().NotBeNull();

            detail2.Currency.Code.Should().Be("SEK");
            detail2.CustomsValue.Should().Be(150);
            detail2.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail2.InvoiceNumber.Should().Be("2345");
            detail2.CountryOfOrigin.Should().Be("JP");
            detail2.CustomsValueString().Should().Be("SEK 150.00");
        }

        [Test]
        public void ShouldNotHaveBRGSection()
        {
            this.Sut.Sections.Any(s => s.CpcScheme == "BRG").Should().BeFalse();
        }
    }
}
