namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.SalesOutlets;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenROWRsns : ContextBase
    {
        private ImportCandidate result;

        [SetUp]
        public async Task SetUp()
        {
            var rsn = new Rsn
            {
                RsnNumber = 12,
                SalesOutlet = TestSalesOutlets.LinnJapan,
                SalesArticle = TestSalesArticles.Akiva,
                Quantity = 1,
                RsnReturns = new List<RsnReturnInformation>()
                {
                    new RsnReturnInformation
                    {
                        CustomsValue = 298271.2m,
                        Currency = TestCurrencies.JapaneseYen
                    }
                }
            };

            this.CurrencyRepository.FindByAsync(Arg.Any<Expression<Func<Currency, bool>>>()).Returns(TestCurrencies.UKPound);
            this.RsnRepository.FindByAsync(Arg.Any<Expression<Func<Rsn, bool>>>()).Returns(rsn);
            this.result = await this.Sut.CreateImportBook(new List<int> { 12 }, null, null, new Employee());
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
            candidate.LineType.Should().Be("RSN");
            candidate.Rsn.Should().NotBeNull();
            candidate.OrderDescription.Should().Be(TestSalesArticles.Akiva.Description);
            candidate.TariffCode.Should().Be(TestSalesArticles.Akiva.Tariff.TariffCode);
            candidate.CountryOfOrigin.Should().Be(TestSalesArticles.Akiva.CountryOfOrigin);
        }

        [Test]
        public void ShouldMakeCorrectInvoiceCandidates()
        {
            this.result.InvoiceDetailCandidates.Should().NotBeNull();
            this.result.InvoiceDetailCandidates.Count.Should().Be(1);
            var candidate = this.result.InvoiceDetailCandidates.First();
            candidate.InvoiceNumber.Should().Be("12");
            candidate.InvoiceValue.Should().Be(298271.2m);
            candidate.Currency.Should().Be(TestCurrencies.JapaneseYen);
        }
    }
}
