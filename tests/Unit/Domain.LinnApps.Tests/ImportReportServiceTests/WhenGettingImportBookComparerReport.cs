namespace Linn.Stores2.Domain.LinnApps.Tests.ImportReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using global::Linn.Common.Reporting.Models;
    using global::Linn.Stores2.Domain.LinnApps.External;
    using global::Linn.Stores2.Domain.LinnApps.Logistics;
    using global::Linn.Stores2.Domain.LinnApps.Reports;

    using Imports;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Logistics;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingImportBookComparerReport : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        [SetUp]
        public async Task SetUp()
        {
            var customCodes = new List<string> { "1", "2", "3" };

            var values = new List<ImportBook>
            {
                new ImportBook
                {
                    Id = 4,
                    CustomsEntryCode = "4",
                    DateCreated = 21.January(2005),
                    Supplier = new Supplier { Id = 1, Name = "Supplier 1", CountryCode = "UK" },
                    CurrencyCode = "GBP",
                    TotalImportValue = 20.23m,
                    OrderDetails = new List<ImportBookOrderDetail>
                    {
                        new ImportBookOrderDetail
                        {
                            TariffCode = "57",
                            OrderValue = 19.56m,
                            CountryOfOrigin = "UK",
                            VatValue = 10.10m,
                            ImportBookCpcNumber = new ImportBookCpcNumber { CpcNumber = 4, Description = "CPC 4" }
                        }
                    }
                },
            };

            this.ImportBookRepository.FilterByAsync(Arg.Any<Expression<Func<ImportBook, bool>>>())
                .Returns(values);

            this.results = await this.Sut.GetImportBookComparerReport(20.January(2005), 22.January(2005), customCodes);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var report = this.results.ElementAt(0);
            report.Should().NotBeNull();

            report.GetGridTextValue(0, report.ColumnIndex("customsEntryCode")).Should().Be("4");
            report.GetGridTextValue(0, report.ColumnIndex("consignor")).Should().Be("Supplier 1");
            report.GetGridTextValue(0, report.ColumnIndex("countryOfDispatch")).Should().Be("UK");
            report.GetGridTextValue(0, report.ColumnIndex("commodityCode")).Should().Be("57");
            report.GetGridTextValue(0, report.ColumnIndex("cpc")).Should().Be("CPC 4");
            report.GetGridTextValue(0, report.ColumnIndex("itemPrice")).Should().Be("20.23");
            report.GetGridTextValue(0, report.ColumnIndex("vatValue")).Should().Be("10.10");
        }
    }
}
