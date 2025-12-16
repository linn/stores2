namespace Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests
{
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyEuDespatchReport : ContextBase
    {
        private Common.Reporting.Models.ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = new Common.Reporting.Models.ResultsModel { ReportTitle = new Common.Reporting.Models.NameModel("Title") };

            this.DailyEuReportService
                .GetDailyEuDespatchReport(
                    NSubstitute.Arg.Any<string>(),
                    NSubstitute.Arg.Any<string>())
                .Returns(this.result);

            this.Response = this.Client.Get(
                $"stores2/reports/daily-eu-dispatch?fromDate={2.December(2025).ToString("o")}&toDate={10.December(2025).ToString("o")}",
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
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
            var resource = this.Response.DeserializeBody<Common.Reporting.Resources.ReportResultResources.ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}