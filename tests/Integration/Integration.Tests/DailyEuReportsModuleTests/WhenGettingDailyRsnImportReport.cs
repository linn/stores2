namespace Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyRsnImportReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = new ResultsModel { ReportTitle = new NameModel("Title") };

            this.DailyEuReportService.GetDailyEuRsnImportReport(Arg.Any<string>(), Arg.Any<string>())
                .Returns(this.result);

            this.Response = this.Client.Get(
                $"/stores2/reports/daily-eu-rsn-import?fromDate={2.December(2025):o}&toDate={10.December(2025):o}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
