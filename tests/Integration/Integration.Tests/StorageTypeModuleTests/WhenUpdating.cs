namespace Linn.Stores2.Integration.Tests.StorageTypeModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private StorageType storageType;

        private StorageTypeResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.storageType = new StorageType("TESTCODE", "A TEST STOCKPOOL");

            this.updateResource = new StorageTypeResource
                                      {
                                          StorageTypeCode = "TESTCODE", Description = "A NEW DESCRIPTION",
                                      };

            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);
            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/storage-types/{this.storageType.StorageTypeCode}",
                this.updateResource).Result;
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
            this.DbContext.StorageTypes.First(x => x.StorageTypeCode == this.storageType.StorageTypeCode).Description
                .Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<StorageType>();
            resource.Description.Should().Be("A NEW DESCRIPTION");
        }
    }
}
