using System.Linq;

namespace Linn.Stores2.Domain.LinnApps.Tests.StockReportServiceTests
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

    public class WhenGettingLabourSummary : ContextBase
    {
        private IEnumerable<ResultsModel> result;

        [SetUp]
        public async Task SetUp()
        {
            var summaries = new List<LabourHoursSummary>()
            {
                new LabourHoursSummary
                {
                    TransactionMonth = 1.August(2025),
                    StockTransactions = 906.54m,
                    AlternativeBuildHours = 3404.73m
                },
                new LabourHoursSummary
                {
                    TransactionMonth = 1.September(2025),
                    StockTransactions = 294.7m,
                    AlternativeBuildHours = 3850.29m
                }
            };

            this.LabourHoursSummaryRepository.FilterByAsync(Arg.Any<Expression<Func<LabourHoursSummary, bool>>>())
                .Returns(summaries);

            this.result = await this.Sut.GetLabourHoursSummaryReport(1.August(2025), 1.September(2025), "LINN");
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Should().NotBeNull();
            var summary = this.result.ToList().FirstOrDefault();
            summary.ReportTitle.DisplayValue.Should().Be("Labour Hours Aug-25 to Sep-25");
            summary.Columns.Should().HaveCount(10);
            summary.Rows.Should().HaveCount(2);
            summary.GetGridTextValue(0, 0).Should().Be("Aug-25");
            summary.GetGridValue(0, 4).Should().Be(906.54m);
            summary.GetGridValue(0, 5).Should().Be(3404.73m);
            summary.GetGridTextValue(1, 0).Should().Be("Sep-25");
            summary.GetGridValue(1, 4).Should().Be(294.7m);
            summary.GetGridValue(1, 5).Should().Be(3850.29m);
        }
    }
}
