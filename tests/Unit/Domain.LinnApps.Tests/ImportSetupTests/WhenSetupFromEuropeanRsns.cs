namespace Linn.Stores2.Domain.LinnApps.Tests.ImportSetupTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.SalesOutlets;

    using NUnit.Framework;

    public class WhenSetupFromEuropeanRsns
    {
        private ImportSetup sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ImportSetup();
            this.sut.AddRsn(new Rsn
                            {
                                RsnNumber = 12,
                                SalesOutlet = TestSalesOutlets.TonlagetHifi,
                                SalesArticle = TestSalesArticles.Akiva,
                                Quantity = 1,
                                ExportReturnDetails = new List<ExportReturnDetail>()
                                                      {
                                                          new ExportReturnDetail
                                                          {
                                                              CustomsValue = 421.28m,
                                                              ExportReturn = new ExportReturn
                                                                             {
                                                                                 Currency = TestCurrencies.SwedishKrona
                                                                             }
                                                          }
                                                      }
                            });
        }

        [Test]
        public void ShouldMakeCorrectOrderCandidates()
        {
            this.sut.OrderDetailCandidates().Should().NotBeNull();
            this.sut.OrderDetailCandidates().Count.Should().Be(1);
            var candidate = this.sut.OrderDetailCandidates().First();
            candidate.LineType.Should().Be("RSN");
            candidate.Rsn.Should().NotBeNull();
            candidate.OrderDescription.Should().Be(TestSalesArticles.Akiva.Description);
            candidate.TariffCode.Should().Be(TestSalesArticles.Akiva.Tariff.TariffCode);
            candidate.CountryOfOrigin.Should().Be(TestSalesArticles.Akiva.CountryOfOrigin);
        }

        [Test]
        public void ShouldMakeCorrectInvoiceCandidates()
        {
            this.sut.InvoiceDetailCandidates().Should().NotBeNull();
            this.sut.InvoiceDetailCandidates().Count.Should().Be(1);
            var candidate = this.sut.InvoiceDetailCandidates().First();
            candidate.InvoiceNumber.Should().Be("12");
            candidate.InvoiceValue.Should().Be(421.28m);
            candidate.Currency.Should().Be(TestCurrencies.SwedishKrona);
        }
    }
}
