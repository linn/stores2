namespace Linn.Stores2.Integration.Tests.StockTransactionModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Integration.Tests.CarrierModuleTests;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = new ResultsModel { ReportTitle = new NameModel("Stock Transaction List") };

            this.StoresTransViewerReportService
                .StoresTransViewerReport(
                    null,
                    null, 
                    null,
                    null,
                    "STLDI2")
                .Returns(this.result);

            this.Response = this.Client.Get(
                "/stores2/stores-trans-viewer/report?functionCode=STLDI2",
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
        public void ShouldReturnNoResults()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("Stock Transaction List");
            resource.Should().NotBeNull();
        }
    }
}
