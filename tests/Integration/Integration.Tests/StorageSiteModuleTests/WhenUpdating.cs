namespace Linn.Stores2.Integration.Tests.StorageSiteModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        [SetUp]
        public void Setup()
        {
            var resource = new StorageSiteResource
                               {
                                   SiteCode = "CODE",
                                   Description = "NEW DESC",
                                   SitePrefix = "Q"
                               };

            this.Repository.FindByIdAsync("CODE").Returns(new StorageSite("CODE", "DESC", "P"));
            this.Response = this.Client.PutAsJsonAsync("/stores2/storage/sites/CODE", resource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
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
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageSiteResource>();
            resource.SiteCode.Should().Be("CODE");
            resource.Description.Should().Be("NEW DESC");
            resource.SitePrefix.Should().Be("Q");
        }
    }
}
