namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyRsnImportReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public async Task SetUp()
        {
            var values = new List<DailyEuRsnImportReport>
                                {
                                    new DailyEuRsnImportReport
                                        {
                                            InvoiceNumber = 1,
                                            RsnNumber = 101,
                                            Currency = "USD",
                                            DocumentDate = 9.December(2025)
                                        },
                                    new DailyEuRsnImportReport
                                        {
                                            InvoiceNumber = 2,
                                            RsnNumber = 102,
                                            Currency = "GBP",
                                            DocumentDate = 8.December(2025)
                                        },
                                    new DailyEuRsnImportReport
                                        {
                                            InvoiceNumber = 3,
                                            RsnNumber = 103,
                                            Currency = "EUR",
                                            DocumentDate = 15.December(2025),
                                        },
                                };

            this.DailyEuRsnImportRepository.FilterByAsync(Arg.Any<Expression<Func<DailyEuRsnImportReport, bool>>>())
                .Returns(values);

            this.result = await this.Sut.GetDailyEuImportRsnReport(1.December(2025).ToString("o"), 20.December(2025).ToString("o"));
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Should().NotBeNull();
            var summary = this.result;

            summary.Rows.Should().HaveCount(3);

            summary.GetGridValue(0, 0).Should().Be(1);
            summary.GetGridValue(1, 0).Should().Be(2);
            summary.GetGridValue(2, 0).Should().Be(3);

            summary.GetGridValue(0, 5).Should().Be(101);
            summary.GetGridValue(1, 5).Should().Be(102);
            summary.GetGridValue(2, 5).Should().Be(103);

            summary.GetGridTextValue(0, 12).Should().Be("USD");
            summary.GetGridTextValue(1, 12).Should().Be("GBP");
            summary.GetGridTextValue(2, 12).Should().Be("EUR");
        }
    }
}
