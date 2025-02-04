namespace Linn.Stores2.Integration.Tests.GoodsInModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private ResultsModel result;

        private GoodsInLogEntry firstLogEntry;

        private GoodsInLogEntry secondLogEntry;

        [SetUp]
        public void SetUp()
        {
            this.result = new ResultsModel { ReportTitle = new NameModel("Goods In Log") };

            this.firstLogEntry = new GoodsInLogEntry
                                     {
                                         ArticleNumber = "TEST PART/X/1",
                                         BookInRef = 12345,
                                         DateCreated = DateTime.Today,
                                         Comments = "Push pull",
                                         CreatedBy = 33156,
                                         DemLocation = "R1/2/4",
                                         ErrorMessage = string.Empty,
                                         Id = 145,
                                         Quantity = 5,
                                         OrderNumber = 2,
                                         LoanLine = 1,
                                         ReqNumber = 1556,
                                         StoragePlace = "SAFE/STORAGE/1/X",
                                         Processed = "Y",
                                         State = "A",
                                         LoanNumber = 1234,
                                         LogCondition = "A CONDITION LIKE NO OTHER",
                                         ManufacturersPartNumber = "EXAMPLE/PART6",
                                         OrderLine = 1243,
                                         ProductAnalysisCode = "PRODUCT/A/B",
                                         ReqLine = 1,
                                         RsnAccessories = "KMON/1",
                                         RsnNumber = 12345,
                                         SerialNumber = 999987,
                                         SernosTref = 1,
                                         StorageType = "C",
                                         TransactionType = "O",
                                         WandString = "WAND"
                                     };

            this.secondLogEntry = new GoodsInLogEntry
                                     {
                                         ArticleNumber = "SECOND TEST PART/X/1",
                                         BookInRef = 54321,
                                         DateCreated = DateTime.Today,
                                         Comments = "Pull push",
                                         CreatedBy = 33157,
                                         DemLocation = "R4/2/4",
                                         ErrorMessage = string.Empty,
                                         Id = 146,
                                         Quantity = 8,
                                         OrderNumber = 1,
                                         LoanLine = 2,
                                         ReqNumber = 6551,
                                         StoragePlace = "SAFE/STORAGE/2/X",
                                         Processed = "N",
                                         State = "A",
                                         LoanNumber = 1234,
                                         LogCondition = "APART FROM THIS ONE",
                                         ManufacturersPartNumber = "EXAMPLE/PART7",
                                         OrderLine = 1243,
                                         ProductAnalysisCode = "PRODUCT/B/A",
                                         ReqLine = 1,
                                         RsnAccessories = "HEWI/1",
                                         RsnNumber = 54321,
                                         SerialNumber = 78999,
                                         SernosTref = 2,
                                         StorageType = "O",
                                         TransactionType = "C",
                                         WandString = "WARV"
                                     };

            this.GoodsInLogReportService
                .GoodsInLogReport(
                    null,
                    null, 
                    33156,
                    null,
                    null, 
                    null,
                    null,
                    null)
                .Returns(this.result);

            this.Response = this.Client.Get(
                "/stores2/goods-in-log/report?createdBy=33156",
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
            resource.ReportResults.First().title.displayString.Should().Be("Goods In Log");
            resource.Should().NotBeNull();
        }
    }
}
