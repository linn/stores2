﻿namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingAllLocations : ContextBase
    {
        private StorageSite eaglesham;
        private StorageSite secretBunker;
        private StorageLocation location1;
        private StorageLocation location2;
        private StorageLocation location3;

        [SetUp]
        public void SetUp()
        {
            this.eaglesham = new StorageSite("A", "SUPPLIER STORES", null)
                                 {
                                     StorageAreas = new List<StorageArea>
                                                        {
                                                            new StorageArea { StorageAreaCode = "FACTORY", Description = "FACTORY AREA", SiteCode = "EAGLESHAM", AreaPrefix = "FA" },
                                                            new StorageArea { StorageAreaCode = "PCB", Description = "PCB AREA", SiteCode = "EAGLESHAM", AreaPrefix = "PCB" }
                                                        }
                                 };

            this.secretBunker = new StorageSite("B", "SUPPLIER STORES", null)
                                    {
                                        StorageAreas = new List<StorageArea>
                                                           {
                                                               new StorageArea { StorageAreaCode = "VAULT", Description = "THE VAULT", SiteCode = "EAGLESHAM", AreaPrefix = "VA" }
                                                           }
                                    };

            this.DbContext.StorageSites.AddAndSave(this.DbContext, this.eaglesham);
            this.DbContext.StorageSites.AddAndSave(this.DbContext, this.secretBunker);

            this.location1 = new StorageLocation
            {
                LocationId = 1,
                LocationCode = "E-FA-FLOOR",
                SiteCode = this.eaglesham.Code,
                StorageAreaCode = "FACTORY",
                Description = "FINAL ASSEMBLY FLOOR"
            };

            this.location2 = new StorageLocation
            {
                LocationId = 2,
                LocationCode = "E-PCB-ATE",
                SiteCode = this.eaglesham.Code,
                StorageAreaCode = "FACTORY",
                Description = "BOARDS A/W ATE TEST"
            };

            this.location3 = new StorageLocation
            {
                LocationId = 3,
                LocationCode = "B-VA-ULT",
                SiteCode = this.eaglesham.Code,
                StorageAreaCode = "VAULT",
                Description = "SECRET BUNKER VAULT"
            };

            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.location1);
            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.location2);
            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.location3);
            this.DbContext.SaveChanges();

            this.Response = this.Client.Get(
                "/stores2/storage/locations",
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
            var resources = this.Response.DeserializeBody<IEnumerable<StorageLocationResource>>().ToList();
            resources.Count.Should().Be(3);
            resources.Single(loc => loc.LocationCode == "E-FA-FLOOR").Should().NotBeNull();
            resources.Single(loc => loc.LocationCode == "E-PCB-ATE").Should().NotBeNull();
            resources.Single(loc => loc.LocationCode == "B-VA-ULT").Should().NotBeNull();
            resources.Single(loc => loc.LocationCode == "E-FA-FLOOR").Description.Should().Be("FINAL ASSEMBLY FLOOR");
        }
    }
}
