namespace Linn.Stores2.Integration.Tests.GoodsInModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.GoodsIn;

    using NUnit.Framework;

    public class WhenSearchingAndSpecifyingOrderNumber : ContextBase
    {
        private GoodsInLogEntry firstLogEntry;

        private GoodsInLogEntry secondLogEntry;

        [SetUp]
        public void SetUp()
        {
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
                                         OrderNumber = 0142,
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
                                         CreatedBy = 33156,
                                         DemLocation = "R4/2/4",
                                         ErrorMessage = string.Empty,
                                         Id = 146,
                                         Quantity = 8,
                                         OrderNumber = 0141,
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

            this.DbContext.GoodsInLogEntries.AddAndSave(this.DbContext, this.firstLogEntry);
            this.DbContext.GoodsInLogEntries.AddAndSave(this.DbContext, this.secondLogEntry);
            this.DbContext.SaveChanges();

            this.Response = this.Client.Get(
                "/stores2/goods-in/log?orderNumber=0141",
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
            var resource = this.Response.DeserializeBody<IEnumerable<GoodsInLogEntryResource>>();
            resource.Should().NotBeNull();
            resource.Count().Should().Be(1);
            resource.SingleOrDefault().OrderNumber.Should().Be(0141);
        }
    }
}
