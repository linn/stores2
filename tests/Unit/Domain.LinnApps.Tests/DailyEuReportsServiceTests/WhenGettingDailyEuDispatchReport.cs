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

        private Expbook expbook;

        [SetUp]
        public async Task SetUp()
        {
            var values = new List<DailyEuDespatchReport>
                                {
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 1,
                                            ProductId = "Article 1",
                                            Currency = "USD",
                                            DateCreated = 9.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 2,
                                            ProductId = "Article 2",
                                            Currency = "GBP",
                                            DateCreated = 8.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 3,
                                            ProductId = "Article 3",
                                            Currency = "EUR",
                                            DateCreated = 15.December(2025),
                                        },
                                };

            this.expbook = new Expbook
                               {
                                   Id = 1,
                                   AddressId = 2,
                                   Address = new Address(
                                       "addresse",
                                       "Line 1",
                                       "Line 2",
                                       "Line 3",
                                       "Line 4",
                                       "G44 123",
                                       new Country())
                               };

            this.DailyEuDespatchRepository.FilterByAsync(Arg.Any<Expression<Func<DailyEuDespatchReport, bool>>>())
                .Returns(values);

            this.ExpbookRepository.FindByIdAsync(Arg.Any<int>()).Returns(this.expbook);

            this.result = await this.Sut.GetDailyEuDespatchReport(1.December(2025).ToString("o"), 20.December(2025).ToString("o"));
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Should().NotBeNull();
            var summary = this.result;

            summary.Rows.Should().HaveCount(3);

            summary.GetGridTextValue(0, 0).Should().Be("Line 1");
            summary.GetGridTextValue(1, 0).Should().Be("Line 1");
            summary.GetGridTextValue(2, 0).Should().Be("Line 1");

            summary.GetGridTextValue(0, 3).Should().Be("Article 1");
            summary.GetGridTextValue(1, 3).Should().Be("Article 2");
            summary.GetGridTextValue(2, 3).Should().Be("Article 3");

            summary.GetGridTextValue(0, 7).Should().Be("USD");
            summary.GetGridTextValue(1, 7).Should().Be("GBP");
            summary.GetGridTextValue(2, 7).Should().Be("EUR");
        }
    }
}
