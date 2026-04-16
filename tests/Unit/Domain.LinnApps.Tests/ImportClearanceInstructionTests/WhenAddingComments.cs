namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

    public class WhenAddingComments : ContextBase
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

            var importBook = new ImportBook(candidate)
            {
                ClearanceComments = "Comment 1\nComment 2"
            };

            this.Sut = new ImportClearanceInstruction(this.Master, "Marvin@tnt.com");

            this.Sut.AddImportBook(importBook, this.AuthNumbers);
        }

        [Test]
        public void ShouldHaveComments()
        {
            this.Sut.HasComments.Should().BeTrue();
            this.Sut.Comments.Should().NotBeNull();
            this.Sut.Comments.Length.Should().Be(2);
        }
    }
}
