﻿namespace Linn.Stores2.Integration.Tests.StorageModuleTests
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

    public class WhenCreatingLocation : ContextBase
    {
        private StorageLocationResource createResource;

        private StorageSite eaglesham;

        private AccountingCompany linn;

        [SetUp]
        public void SetUp()
        {
            this.eaglesham = new StorageSite("EAGLESHAM", "SUPPLIER STORES", null)
                                 {
                                     StorageAreas = new List<StorageArea>
                                                        {
                                                            new StorageArea
                                                                {
                                                                    StorageAreaCode = "FACTORY",
                                                                    Description = "FACTORY AREA",
                                                                    SiteCode = "EAGLESHAM",
                                                                    AreaPrefix = "FA"
                                                                }
                                                        }
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

            this.createResource = new StorageLocationResource
            {
                LocationCode = "E-FA-TOAST",
                Description = "COLIN TOAST RACK",
                AccountingCompany = "LINN",
                SiteCode = "EAGLESHAM",
                StorageAreaCode = "FACTORY",
                MixStatesFlag = "Y",
                AccessibleFlag = "Y",
                StockState = "A",
                TypeOfStock = "A"
            };

            this.Response = this.Client.PostAsJsonAsync($"/stores2/storage/locations", this.createResource).Result;
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
            this.DbContext.StorageLocations
                .First(x => x.LocationCode == this.createResource.LocationCode).Description
                .Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageLocationResource>();
            resource.LocationId.Should().Be(1);
            resource.LocationCode.Should().Be("E-FA-TOAST");
        }
    }
}
