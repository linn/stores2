namespace Linn.Stores2.Integration.Tests.StoragePlaceModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel result;

        private string location1;

        private string location2;

        [SetUp]
        public void SetUp()
        {
            this.result = new ResultsModel { ReportTitle = new NameModel("Title") };
            this.location1 = "P285";
            this.location2 = "P745";

            this.StoragePlaceAuditReportService
                .StoragePlaceAuditReport(Arg.Any<IEnumerable<string>>(), Arg.Any<string>())
                .Returns(this.result);

            this.Response = this.Client.Get(
                $"/stores2/reports/storage-place-audit/report?locationList={this.location1}&locationList={this.location2}",
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
