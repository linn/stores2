namespace Linn.Stores2.Integration.Tests.StorageSiteModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        [SetUp]
        public void Setup()
        {
            var resource = new StorageSiteResource
                               {
                                   SiteCode = "CODE",
                                   Description = "DESC",
                                   SitePrefix = "P"
                               };

            this.Repository.FindByIdAsync("CODE").Returns(new StorageSite("CODE", "DESC", "P"));
            this.Response = this.Client.PostAsJsonAsync("/stores2/storage/sites", resource).Result;
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
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageSiteResource>();
            resource.SiteCode.Should().Be("CODE");
            resource.Description.Should().Be("DESC");
            resource.SitePrefix.Should().Be("P");
        }
    }
}
