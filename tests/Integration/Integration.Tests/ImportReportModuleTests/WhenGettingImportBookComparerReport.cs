namespace Linn.Stores2.Integration.Tests.ImportReportModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    using CsvHelper;

    using Domain.LinnApps;

    using Extensions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

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

            this.ImportReportService.CompareImportBooksWithCsvReport(
                    Arg.Any<List<ImportBookCompareReport>>(),
                    20.January(2005),
                    22.January(2005))
                .Returns(result);



            var csvContent = "Entry Identifier,Clearance Date,Consignor,Country of Dispatch,Commodity Code,CPC,Country of Origin,Invoice Currency,Item Price,Customs Value,VAT Value\n" +
                "TEST001,2005-01-21,Test Supplier,FR,12345678,CPC001,FR,EUR,100.50,95.00,19.00";

            this.Response = this.Client.PostCsv(
                    $"/stores2/import-books/comparer/view?fromDate={20.January(2005):yyyy-MM-dd}&toDate={22.January(2005):yyyy-MM-dd}",
                    csvContent,
                    with =>
                    {
                        with.Accept("application/json");
                    })
                .Result;
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
