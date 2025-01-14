namespace Linn.Stores2.Integration.Tests.StockPoolModuleTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private StockPool stockPool;

        private StockPoolUpdateResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.stockPool = new StockPool(
                "TESTCODE",
                "A TEST STOCKPOOL",
                "14/01/2025",
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

            this.updateResource = new StockPoolUpdateResource
                                      { 
                                          StockPoolDescription = "A NEW DESCRIPTION",
                                          DateInvalid = "22/01/2005",
                                          AccountingCompany = new AccountingCompanyResource 
                                                                  { 
                                                                      Description = "Test Description",
                                                                      Id = 456,
                                                                      Name = "Test Name",
                                                                      Sequence = 1
                                                                  },
                                          AccountingCompanyCode = "456",
                                          Sequence = 1,
                                          StockCategory = "B",
                                          DefaultLocation = 7,
                                          StorageLocation = new StorageLocationResource 
                                                                {
                                                                    LocationCode = "NEWLOC",
                                                                    DefaultStockPool = "NEWDEFAULT",
                                                                    Description = "NEW DESC",
                                                                    LocationId = 7,
                                                                    LocationType = "NEWTYPE",
                                                                    SiteCode = "NEW",
                                                                    StorageType = "NEW"
                                                                },
                                          BridgeId = 6,
                                          AvailableToMrp = "N"
                                        };

            this.DbContext.StockPools.AddAndSave(this.DbContext, this.stockPool);
            this.Response = this.Client.PutAsJsonAsync($"/stores2/stock-pools/{this.stockPool.StockPoolCode}", this.updateResource).Result;
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
        public void ShouldUpdateEntity()
        {
            this.DbContext.StockPools.FirstOrDefault(x => x.StockPoolCode == this.stockPool.StockPoolCode)
                .StockPoolDescription.Should().Be(this.updateResource.StockPoolDescription);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StockPoolResource>();
            resource.StockPoolDescription.Should().Be("A NEW DESCRIPTION");
        }
    }
}
