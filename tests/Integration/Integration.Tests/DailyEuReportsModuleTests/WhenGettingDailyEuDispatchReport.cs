using Linn.Common.Reporting.Models;
using Linn.Common.Reporting.Resources.ReportResultResources;
using NUnit.Framework;
using System.Net;
using System;

namespace Linn.Service.Integration.Tests.RsnReportsModuleTests
{
    using System;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Integration.Tests.DailyEuReportsModuleTests;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyEuDispatchReport : ContextBase
    {
        [SetUp]
        public void Setup()
        {
            var result = new ResultsModel { ReportTitle = new NameModel("TITLE") };


            var fromDate = 15.July(2025);
            var toDate = 21.July(2025);

            this.DailyEuReportService.GetDailyEuDispatchReport(fromDate.ToString("o"), toDate.ToString("o"))
                .Returns(result);

            this.Response = Client.Get(
                $"/stores2/customs/daily/eu/despatch/report?fromDate={fromDate.ToString("o")}&toDate={toDate.ToString("o")}",
                with => { with.Accept("application/json"); }).Result;

        }

        [Test]
        public void ShouldReturnOk()
        {
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
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
            resource.Should().NotBeNull();
            resource.ReportResults.Count.Should().Be(1);
            resource.ReportResults.First().title.displayString.Should().Be("TITLE");
        }
    }
}
