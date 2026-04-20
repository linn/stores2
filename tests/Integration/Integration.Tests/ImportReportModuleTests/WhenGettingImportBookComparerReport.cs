namespace Linn.Stores2.Integration.Tests.ImportReportModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using global::Linn.Common.Reporting.Models;
    using global::Linn.Common.Reporting.Resources.ReportResultResources;
    using global::Linn.Stores2.Integration.Tests.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingImportBookComparerReport : ContextBase
    {
        [SetUp]
        public void Setup()
        {
            var result = new List<ResultsModel>
            {
                new ResultsModel { ReportTitle = new NameModel("TITLE") }
            };

            this.ImportReportService.GetImportBookComparerReport(
                    20.January(2005),
                    22.January(2005),
                    Arg.Any<List<string>>())
                .Returns(result);

            this.Response = Client.Get(
                "/stores2/import-books/comparer/view?"
                + $"&fromDate={20.January(2005):yyyy-MM-dd}"
                + $"&toDate={22.January(2005):yyyy-MM-dd}"
                + "&customEntryCodes=1&customEntryCodes=2",
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
            resource.Should().NotBeNull();
            resource.ReportResults.Count.Should().Be(1);
            resource.ReportResults.First().title.displayString.Should().Be("TITLE");
        }
    }
}
