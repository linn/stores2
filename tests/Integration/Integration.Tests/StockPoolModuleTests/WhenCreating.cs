namespace Linn.Stores2.Integration.Tests.StockPoolModuleTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private StockPoolResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new StockPoolResource
            {
                StockPoolCode = "TESTCODE",
                StockPoolDescription = "A DESCRIPTION",
                DateInvalid = DateTime.Now.ToString("o"),
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
                                          LocationCode = "LOC",
                                          DefaultStockPool = "DEFAULT",
                                          Description = "DESC",
                                          LocationId = 7,
                                          LocationType = "TYPE",
                                          SiteCode = "A",
                                          StorageType = "B"
                                      },
                BridgeId = 6,
                AvailableToMrp = "N"
            };

            this.Response = this.Client.PostAsJsonAsync($"/stores2/stock-pools", this.createResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }
        
        [Test]
        public void ShouldAdd()
        {
            this.DbContext.StockPools
                .FirstOrDefault(x => x.StockPoolCode == this.createResource.StockPoolCode)
                .StockPoolDescription.Should().Be(this.createResource.StockPoolDescription);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StockPoolResource>();
            resource.StockPoolCode.Should().Be("TESTCODE");
            resource.StockPoolDescription.Should().Be("A DESCRIPTION");
        }
    }
}
