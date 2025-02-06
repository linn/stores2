namespace Linn.Stores2.Integration.Tests.StorageTypeModuleTests
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
        private StorageTypeResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new StorageTypeResource
                                      {
                                          StorageTypeCode = "TESTCODE",
                                          Description = "A DESCRIPTION",
                                      };

            this.Response = this.Client.PostAsJsonAsync($"/stores2/storage-types", this.createResource).Result;
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
            this.DbContext.StorageTypes
                .First(x => x.StorageTypeCode == this.createResource.StorageTypeCode).Description
                .Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageTypeResource>();
            resource.StorageTypeCode.Should().Be("TESTCODE");
            resource.Description.Should().Be("A DESCRIPTION");
        }
    }
}
