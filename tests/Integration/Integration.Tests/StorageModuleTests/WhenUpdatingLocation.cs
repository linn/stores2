namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenUpdatingLocation : ContextBase
    {
        private StorageSite eaglesham;

        private AccountingCompany linn;

        private StorageLocation storageLocation;

        private StorageLocationResource updateResource;

        [SetUp]
        public void SetUp()
        {
            var factoryArea = new StorageArea
            {
                StorageAreaCode = "FACTORY", Description = "FACTORY AREA", SiteCode = "EAGLESHAM", AreaPrefix = "FA"
            };

            this.eaglesham = new StorageSite()
            {
                SiteCode = "EAGLESHAM",
                Description = "EAGLESHAM",
                SitePrefix = "E",
                StorageAreas = new List<StorageArea> { factoryArea }
            };

            this.linn = new AccountingCompany
            {
                Id = 1,
                Name = "LINN",
                Description = "Linn Products Ltd"
            };

            this.DbContext.StorageSites.AddAndSave(this.DbContext, this.eaglesham);
            this.DbContext.AccountingCompanies.AddAndSave(this.DbContext, this.linn);
            this.DbContext.SaveChanges();

            this.storageLocation = new StorageLocation(
                1,
                "E-FA-TEST",
                "TOAST DESCRIPTION",
                this.eaglesham,
                factoryArea,
                this.linn,
                "Y",
                "Y",
                "Y",
                "Y",
                "A",
                "A",
                null,
                null);

            this.updateResource = new StorageLocationResource
                                      {
                                          LocationId = 1,
                                          LocationCode = "E-FA-TEST",
                                          AccountingCompany = this.linn.Name,
                                          Description = "THIS IS A TEST",
                                          AccessibleFlag = "Y",
                                          StoresKittableFlag = "Y",
                                          MixStatesFlag = "Y",
                                          TypeOfStock = "A",
                                          StockState = "A",
                                          DefaultStockPool = null,
                                          StorageType = null
                                      };

            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.storageLocation);
            this.Response = this.Client.PutAsJsonAsync($"/stores2/storage/locations/{this.storageLocation.LocationId}", this.updateResource).Result;
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
            this.DbContext.StorageLocations
                .First(x => x.LocationId == this.storageLocation.LocationId).Description
                .Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageLocationResource>();
            resource.Description.Should().Be("THIS IS A TEST");
        }
    }
}
