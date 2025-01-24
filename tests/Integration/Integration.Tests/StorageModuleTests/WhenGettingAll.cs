﻿using FluentAssertions;
using Linn.Stores2.Integration.Tests.Extensions;
using Linn.Stores2.Resources;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Linn.Stores2.Domain.LinnApps.Stock;

namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    public class WhenGettingAll : ContextBase
    {
        private StorageSite eaglesham;

        [SetUp]
        public void SetUp()
        {
            this.eaglesham = new StorageSite()
            {
                SiteCode = "EAGLESHAM", Description = "EAGLESHAM", SitePrefix = "E",
                StorageAreas = new List<StorageArea> { new StorageArea {StorageAreaCode = "FACTORY", Description = "FACTORY AREA", SiteCode = "EAGLESHAM", AreaPrefix = "FA" } }
            };

            this.DbContext.StorageSites.AddAndSave(this.DbContext, this.eaglesham);
            this.DbContext.SaveChanges();

            this.Response = this.Client.Get(
                "/stores2/storage/sites",
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
            var resource = this.Response.DeserializeBody<IEnumerable<StorageSiteResource>>();
            resource.First().SiteCode.Should().Be("EAGLESHAM");
        }
    }
}
