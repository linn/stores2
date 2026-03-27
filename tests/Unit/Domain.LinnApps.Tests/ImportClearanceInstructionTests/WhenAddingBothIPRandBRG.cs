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

    public class WhenAddingBothIPRandBRG : ContextBase
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
                    ReasonCode = "RFR",
                    ReasonCategory = "Repair"
                },
                ImportBookOrderDetails = new List<ImportBookOrderDetail>(),
                ExportReturnDetails = new List<ExportReturnDetail>(),
                RsnReturns = new List<RsnReturnInformation>()
                {
                    new RsnReturnInformation
                    {
                        CustomsValue = 150,
                        Currency = TestCurrencies.SwedishKrona
                    }
                }
            };

            var brgDetail = new ImportOrderDetailCandidate(brgRsn, TestCpcNumbers.BRGCpc);

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
            this.Sut.Invoices.Should().Be("1234,2345");
        }

        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count().Should().Be(2);
            this.Sut.HasIPRAndBRG.Should().BeTrue();
            this.Sut.IPRTotals.First().ToString().Should().Be("SEK 100.00");
            this.Sut.BRGTotals.First().ToString().Should().Be("SEK 150.00");
            this.Sut.ValuePrompt("IPR").Should().Be("IPR Value:");
            this.Sut.ValuePrompt("BRG").Should().Be("BRG Value:");
        }

        [Test]
        public void ShouldHaveIPRSection()
        {
            this.Sut.Sections.Should().NotBeNull();
            this.Sut.Sections.Count.Should().Be(2);

            var section1 = this.Sut.Sections.FirstOrDefault();
            section1.Should().NotBeNull();

            section1.ReasonForImport.Should().Be("Return for Repair");
            section1.CpcNumber.Should().Be("51 00 000");
            section1.CpcScheme.Should().Be("IPR");
            section1.IPR.Should().BeTrue();
            section1.Declaration.Should().StartWith("CDS IPR Reference - GBIPO3830942440020210510102146");
            section1.Details.Should().NotBeNull();
            section1.Details.Should().HaveCount(1);

            var detail1 = section1.Details.FirstOrDefault();
            detail1.Should().NotBeNull();

            detail1.Currency.Code.Should().Be("SEK");
            detail1.CustomsValue.Should().Be(100);
            detail1.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail1.InvoiceNumber.Should().Be("1234");
            detail1.CountryOfOrigin.Should().Be("JP");
            detail1.CustomsValueString().Should().Be("SEK 100.00");
        }

        [Test]
        public void ShouldHaveBRGSection()
        {
            var section2 = this.Sut.Sections.FirstOrDefault(s => s.CpcScheme == "BRG");

            section2.Should().NotBeNull();

            section2.ReasonForImport.Should().Be("Return for Repair");
            section2.CpcNumber.Should().Be("61 10 F05 - BRG");
            section2.CpcScheme.Should().Be("BRG");
            section2.IPR.Should().BeFalse();
            section2.Declaration.Should().StartWith("NIRU / RGR / 336 / 0325");
            section2.Details.Should().NotBeNull();
            section2.Details.Should().HaveCount(1);

            var detail2 = section2.Details.FirstOrDefault();
            detail2.Should().NotBeNull();

            detail2.Currency.Code.Should().Be("SEK");
            detail2.CustomsValue.Should().Be(150);
            detail2.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail2.InvoiceNumber.Should().Be("2345");
            detail2.CountryOfOrigin.Should().Be("JP");
            detail2.CustomsValueString().Should().Be("SEK 150.00");
        }
    }
}
