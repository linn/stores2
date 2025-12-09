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

    public class WhenGettingDailyEuDispatchReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public async Task SetUp()
        {
            var values = new List<DailyEuDespatchReport>
                                {
                                    new DailyEuDespatchReport
                                        {
                                            ExbookId = 1,
                                            ArticleNumber = "Article 1",
                                            Currency = "USD",
                                            DateCreated = 9.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            ExbookId = 2,
                                            ArticleNumber = "Article 2",
                                            Currency = "GBP",
                                            DateCreated = 8.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            ExbookId = 3,
                                            ArticleNumber = "Article 3",
                                            Currency = "EUR",
                                            DateCreated = 15.December(2025),
                                        },
                                };

            this.DailyEuDespatchRepository.FilterByAsync(Arg.Any<Expression<Func<DailyEuDespatchReport, bool>>>())
                .Returns(values);

            this.result = await this.Sut.GetDailyEuDespatchReport(1.December(2025).ToString("o"), 20.December(2025).ToString("o"));
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

            summary.GetGridTextValue(0, 3).Should().Be("Article 1");
            summary.GetGridTextValue(1, 3).Should().Be("Article 2");
            summary.GetGridTextValue(2, 3).Should().Be("Article 3");

            summary.GetGridTextValue(0, 8).Should().Be("USD");
            summary.GetGridTextValue(1, 8).Should().Be("GBP");
            summary.GetGridTextValue(2, 8).Should().Be("EUR");
        }
    }
}
