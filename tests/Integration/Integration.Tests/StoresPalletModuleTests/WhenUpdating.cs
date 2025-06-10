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

    public class WhenUpdating : ContextBase
    {
        private StoresPallet pallet;

        private StoresPalletResource updateResource;

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

            this.pallet = new StoresPallet(
                1,
                "Test-Description",
                this.storageLocation,
                3,
                "Y",
                "Y",
                1,
                "Y",
                1,
                null,
                null,
                123,
                null,
                null,
                "A",
                "A",
                4,
                1,
                "State1,State2",
                "A");

            this.updateResource = new StoresPalletResource
            {
                PalletNumber = 1,
                Description = "A NEW DESCRIPTION",
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
                LocationType = null,
                LocationTypeId = "LOC_TYPE",
                AuditedBy = 123,
                DefaultStockPoolId = "DEFAULT_POOL",
                DefaultStockPool = null,
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

            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet);

            this.Response = this.Client.PutAsJsonAsync($"/stores2/pallets/1", this.updateResource).Result;
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
            this.DbContext.StoresPallets
                .First(x => x.PalletNumber == this.pallet.PalletNumber).Description
                .Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StoresPalletResource>();
            resource.Description.Should().Be("A NEW DESCRIPTION");
        }
    }
}
