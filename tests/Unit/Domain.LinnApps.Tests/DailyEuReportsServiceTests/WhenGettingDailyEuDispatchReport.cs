using Linn.Common.Reporting.Models;
using Linn.Service.Domain.LinnApps;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    public class WhenGettingDailyEuDispatchReport : ContextBase
    {
        private IEnumerable<InterCompanyInvoice> data;

        private ResultsModel results;

        [SetUp]
        public async Task Setup()
        {
            var fromDate = new DateTime(2025, 7, 15);
            var toDate = new DateTime(2025, 7, 21);

            this.data = new List<InterCompanyInvoice>
{
    new InterCompanyInvoice
    {
        DocumentNumber = 1,
        DocumentType = "E",
        DocumentDate = 16.September(2025),
        InvoiceAddress = new Address(),
        DeliveryAddress = new Address(),
        Terms = "CIF",
        Details = new List<InterCompanyInvoiceDetail>
        {
            new InterCompanyInvoiceDetail
            {
                DocumentType = "E",
                DocumentNumber = 1,
                LineNumber = 1,
                ArticleNumber = "A100",
                Tariff = new Tariff { TariffCode = "1234.56", Description = "Widgets" },
                CountryOfOrigin = "GB",
                Quantity = 10,
                Country = new Country("GB", "United Kingdom"),
                UnitPrice = 5.5m,
                Total = 55.0m,
                SalesArticle = new SalesArticle { Weight = 2.5m }
            },
            new InterCompanyInvoiceDetail
            {
                DocumentType = "E",
                DocumentNumber = 1,
                LineNumber = 2,
                ArticleNumber = "A101",
                Tariff = new Tariff { TariffCode = "6543.21", Description = "Gadgets" },
                CountryOfOrigin = "DE",
                Quantity = 5,
                Country = new Country("DE", "Germany"),
                UnitPrice = 7.0m,
                Total = 35.0m,
                SalesArticle = new SalesArticle { Weight = 1.2m }
            }
        }
    },
    new InterCompanyInvoice
    {
        DocumentNumber = 2,
        DocumentType = "E",
        DocumentDate = 16.September(2025),
        InvoiceAddress = new Address(),
        DeliveryAddress = new Address(),
        Terms = "FOB",
        Details = new List<InterCompanyInvoiceDetail>
        {
            new InterCompanyInvoiceDetail
            {
                DocumentType = "E",
                DocumentNumber = 2,
                LineNumber = 1,
                ArticleNumber = "A200",
                Tariff = new Tariff { TariffCode = "7890.12", Description = "Thingamajigs" },
                CountryOfOrigin = "FR",
                Quantity = 3,
                Country = new Country("FR", "France"),
                UnitPrice = 15.0m,
                Total = 45.0m,
                SalesArticle = new SalesArticle { Weight = 3.0m }
            }
        }
    }
};


            this.InterCompanyInvoiceRepository
                .FilterByAsync(Arg.Any<Expression<Func<InterCompanyInvoice, bool>>>())
                .Returns(Task.FromResult<IList<InterCompanyInvoice>>(this.data.ToList()));

            this.results = await this.Sut.GetDailyEuDispatchReport(fromDate.ToString("o"), toDate.ToString("o"));
        }

        [Test]
        public void ShouldReturnReports()
        {
            this.results.Should().NotBe(null);
        }

        [Test]
        public void ShouldCalculateAllocTotals()
        {
            this.results.Rows.Count().Should().Be(3);

            this.results.GetGridTextValue(0, 2).Should().Be("1"); // Row 1 RsnNumber
            this.results.GetGridTextValue(1, 2).Should().Be(string.Empty);  // Row 2 RsnNumber
            this.results.GetGridTextValue(2, 2).Should().Be("2");  // Row 2 RsnNumber

            this.results.GetGridTextValue(0, 6).Should().Be("GB"); // Row 1 Sales Article
            this.results.GetGridTextValue(1, 6).Should().Be("DE");  // Row 2 Sales Article
            this.results.GetGridTextValue(2, 6).Should().Be("FR");  // Row 2 Sales Article

            this.results.GetGridTextValue(0, 10).Should().Be("55.0"); // Row 1 Sales Reason
            this.results.GetGridTextValue(1, 10).Should().Be("35.0");  // Row 2 Sales Reason
            this.results.GetGridTextValue(2, 10).Should().Be("45.0");  // Row 2 Sales Reason
        }
    }
}
