namespace Linn.Stores2.Integration.Tests.StoresPalletModuleTests
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

    public class WhenCreating : ContextBase
    {
        private StoresPalletResource createResource;

        private StockPool stockPool;

        private StorageLocation storageLocation;

        private LocationType locationType;

        [SetUp]
        public void SetUp()
        {
            this.stockPool = new StockPool 
                                 { 
                                     StockPoolCode = "DEFAULT_POOL", 
                                     StockPoolDescription = "Default Pool Description",
                                 };

            this.storageLocation = new StorageLocation
            {
                LocationId = 3,
                Description = "Test Location"
            };

            this.locationType = new LocationType
            {
                Code = "LOC_TYPE",
                Description = "Location Type Description",
            };


            this.createResource = new StoresPalletResource
            {
                                         PalletNumber = 1,
                                         Description = "Test-Description",
                                         StorageLocationId = 3,
                                         StorageLocation = new StorageLocationResource
                                                          {
                                                              LocationId = this.storageLocation.LocationId, 
                                                              Description = this.storageLocation.Description
                                         },
                                         DateInvalid = DateTime.Today.ToString("o"),
                                         DateLastAudited = DateTime.Today.ToString("o"),
                                         Accessible = "Y",
                                         StoresKittable = "Y",
                                         StoresKittingPriority = 1,
                                         SalesKittable = "Y",
                                         SalesKittingPriority = 1,
                                         AllocQueueTime = DateTime.Now.ToString("o"),
                                         LocationType = new LocationTypeResource 
                                                            {
                                                                Code = this.locationType.Code,
                                                                Description = this.locationType.Description,
                                                            },
                                         LocationTypeId = "LOC_TYPE",
                                         AuditedBy = 123,
                                         DefaultStockPoolId = "DEFAULT_POOL",
                                         DefaultStockPool = new StockPoolResource
                                                            {
                                                                StockPoolCode = this.stockPool.StockPoolCode,
                                                                StockPoolDescription = this.stockPool.StockPoolDescription,
                                                            },
                                         StockType = "TypeA",
                                         StockState = "StateA",
                                         AuditOwnerId = 456,
                                         AuditFrequencyWeeks = 4,
                                         AuditedByDepartmentCode = "DeptA",
                                         MixStates = "State1,State2",
                                         Cage = "A"
                                      };

            this.DbContext.LocationTypes.AddAndSave(this.DbContext, this.locationType);

            this.DbContext.StockPools.AddAndSave(this.DbContext, this.stockPool);

            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.storageLocation);

            this.Response = this.Client.PostAsJsonAsync("/stores2/pallets", this.createResource).Result;
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
            this.DbContext.StoresPallets
                .FirstOrDefault(x => x.PalletNumber == this.createResource.PalletNumber)
                .Description.Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StoresPalletResource>();
            resource.PalletNumber.Should().Be(1);
            resource.Description.Should().Be("Test-Description");
        }
    }
}
