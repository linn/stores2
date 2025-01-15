namespace Linn.Stores2.Integration.Tests.StockPoolModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingAll : ContextBase
    {
        private StockPool stockPool;

        [SetUp]
        public void SetUp()
        {
            this.stockPool = new StockPool(
                "TESTCODE",
                "A TEST STOCKPOOL",
                DateTime.Now.ToString("o"),
                new AccountingCompany
                    {
                        Description = "LINN LTD",
                        Id = 123,
                        Name = "E DONN",
                        Sequence = 56
                    },
                123,
                "C",
                1,
                new StorageLocation
                    {
                        DateInvalid = DateTime.Today,
                        LocationCode = "HERE",
                        DefaultStockPool = "DEFAULT",
                        Description = "THE DEFAULT DESC",
                        LocationId = 6,
                        LocationType = "STANDARD",
                        SiteCode = "LINN",
                        StorageType = "NEW"
                    },
                5,
                "Y");

            this.DbContext.StockPools.AddAndSave(this.DbContext, this.stockPool);

            this.Response = this.Client.Get(
                "/stores2/stock-pools",
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
            var resource = this.Response.DeserializeBody<IEnumerable<StockPoolResource>>();
            resource.First().StockPoolCode.Should().Be("TESTCODE");
            resource.First().StockPoolDescription.Should().Be("A TEST STOCKPOOL");
            resource.First().BridgeId.Should().Be(5);
        }
    }
}
