namespace Linn.Stores2.Integration.Tests.StockReportModuleTests
{
    using System.Linq;
    using FluentAssertions;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Integration.Tests.Extensions;
    using NUnit.Framework;
    using System.Net;
    using NSubstitute;

    public class WhenGettingLabourHoursInStockReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = new ResultsModel { ReportTitle = new NameModel("Title") };
            var jobref = "AAAAAA";
            var accountingCompany = "LINN";

            this.StockReportService.GetStockInLabourHours(jobref, accountingCompany, true)
                .Returns(this.result);

            this.Response = this.Client.Get(
                $"/stores2/reports/labour-hours-in-stock/report?jobref={jobref}&accountingCompany={accountingCompany}",
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
