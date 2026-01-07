namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyRsnImportReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public async Task SetUp()
        {
            var values = new List<DailyEuRsnImport>
                                {
                                    new DailyEuRsnImport
                                        {
                                            InvoiceNumber = 1,
                                            RsnNumber = 101,
                                            Currency = "USD",
                                            Weight = 12m,
                                            Pieces = 2,
                                            Width = 1,
                                            Height = 2,
                                            PartNumber = "P1",
                                            PartDescription = "P1 Desc",
                                            Depth = 3,
                                            DocumentDate = 9.December(2025),
                                            Retailer = "Ret 1",
                                            ReturnReason = "REPAIR",
                                            TariffCode = "8518 9000 00",
                                            CustomsCpcNumber = "23 00",
                                            CountryOfOrigin = "GB",
                                            Quantity = 2,
                                            CustomsValue = 123.45m
                                        },
                                    new DailyEuRsnImport
                                        {
                                            InvoiceNumber = 2,
                                            RsnNumber = 102,
                                            Currency = "GBP",
                                            DocumentDate = 8.December(2025)
                                        },
                                    new DailyEuRsnImport
                                        {
                                            InvoiceNumber = 3,
                                            RsnNumber = 103,
                                            Currency = "EUR",
                                            DocumentDate = 15.December(2025),
                                        },
                                };

            this.DailyEuRsnImportRepository.FilterByAsync(Arg.Any<Expression<Func<DailyEuRsnImport, bool>>>())
                .Returns(values);

            this.result = await this.Sut.GetDailyEuRsnImportReport(1.December(2025), 20.December(2025));
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Rows.Should().HaveCount(3);

            this.result.GetGridTextValue(0, this.result.ColumnIndex("intercompanyInvoice")).Should().Be("1");
            this.result.GetGridValue(0, this.result.ColumnIndex("pieces")).Should().Be(2);
            this.result.GetGridValue(0, this.result.ColumnIndex("weight")).Should().Be(12m);
            this.result.GetGridTextValue(0, this.result.ColumnIndex("dims")).Should().Be("1 x 2 x 3");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("retailerDetails")).Should().Be("Ret 1");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("rsnNumber")).Should().Be("101");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("partNumber")).Should().Be("P1");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("description")).Should().Be("P1 Desc");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("returnReason")).Should().Be("REPAIR");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("customsCpcNumber")).Should().Be("23 00");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("tariffCode")).Should().Be("8518 9000 00");
            this.result.GetGridTextValue(0, this.result.ColumnIndex("countryOfOrigin")).Should().Be("GB");
            this.result.GetGridValue(0, this.result.ColumnIndex("quantity")).Should().Be(2m);
            this.result.GetGridTextValue(0, this.result.ColumnIndex("currency")).Should().Be("USD");
            this.result.GetGridValue(0, this.result.ColumnIndex("customsValue")).Should().Be(123.45m);

            this.result.GetGridTextValue(1, this.result.ColumnIndex("intercompanyInvoice")).Should().Be("2");
            this.result.GetGridTextValue(1, 5).Should().Be("102");
            this.result.GetGridTextValue(1, 13).Should().Be("GBP");

            this.result.GetGridTextValue(2, this.result.ColumnIndex("intercompanyInvoice")).Should().Be("3");
            this.result.GetGridTextValue(2, 5).Should().Be("103");
            this.result.GetGridTextValue(2, 13).Should().Be("EUR");
        }
    }
}
