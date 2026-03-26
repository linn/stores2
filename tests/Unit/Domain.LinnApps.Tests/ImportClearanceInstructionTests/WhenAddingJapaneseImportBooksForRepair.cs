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

    public class WhenAddingJapaneseImportBooksForRepair : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var rsn1 = new Rsn
            {
                RsnNumber = 1234,
                SalesOutlet = TestSalesOutlets.LinnJapan,
                AccountId = TestSalesOutlets.LinnJapan.AccountId,
                OutletNumber = TestSalesOutlets.LinnJapan.OutletNumber,
                Quantity = 1,
                Ipr = "Y",
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
                        CustomsValue = 100000000,
                        Currency = TestCurrencies.JapaneseYen
                    }
                }
            };

            var rsn2 = new Rsn
            {
                RsnNumber = 12345,
                SalesOutlet = TestSalesOutlets.LinnJapan,
                AccountId = TestSalesOutlets.LinnJapan.AccountId,
                OutletNumber = TestSalesOutlets.LinnJapan.OutletNumber,
                Quantity = 1,
                Ipr = "Y",
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
                        CustomsValue = 200000000,
                        Currency = TestCurrencies.JapaneseYen
                    }
                }
            };

            var orderDetail1 = new ImportOrderDetailCandidate(rsn1, TestCpcNumbers.IPRCpc);
            var orderDetail2 = new ImportOrderDetailCandidate(rsn2, TestCpcNumbers.IPRCpc);

            var candidate = new ImportCandidate
            {
                Id = 1,
                TransportBillNumber = "12345",
                Supplier = TestSuppliers.LinnJapan,
                Carrier = TestSuppliers.Fedex,
                Currency = TestCurrencies.JapaneseYen,
                BaseCurrency = TestCurrencies.UKPound,
                CreatedBy = TestEmployees.SophlyBard,
                OrderDetailCandidates = new List<ImportOrderDetailCandidate> { orderDetail1, orderDetail2 },
                InvoiceDetailCandidates = new List<ImportInvoiceDetailCandidate>
                {
                    new ImportInvoiceDetailCandidate(rsn1),
                    new ImportInvoiceDetailCandidate(rsn2)
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
            this.Sut.Supplier.Name.Should().Be(TestSuppliers.LinnJapan.Name);
            this.Sut.Carrier.Should().NotBeNull();
            this.Sut.Invoices.Should().Be("1234,12345");
        }

        [Test]
        public void ShouldHaveCorrectTotals()
        {
            this.Sut.Totals.Should().NotBeNull();
            this.Sut.Totals.Count().Should().Be(1);
            this.Sut.Totals.First().ToString().Should().Be("JPY 300,000,000");
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
            section.Details.Should().HaveCount(2);

            var detail = section.Details.FirstOrDefault();
            detail.Should().NotBeNull();

            detail.Currency.Code.Should().Be("JPY");
            detail.CustomsValue.Should().Be(100000000);
            detail.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail.InvoiceNumber.Should().Be("1234");
            detail.CountryOfOrigin.Should().Be("JP");
            detail.CustomsValueString().Should().Be("JPY 100,000,000");

            var detail2 = section.Details.LastOrDefault();
            detail2.Should().NotBeNull();

            detail2.Currency.Code.Should().Be("JPY");
            detail2.CustomsValue.Should().Be(200000000);
            detail2.Description.Should().Be(TestSalesArticles.Akiva.Description);
            detail2.InvoiceNumber.Should().Be("12345");
            detail2.CountryOfOrigin.Should().Be("JP");
            detail2.CustomsValueString().Should().Be("JPY 200,000,000");
        }
    }
}
