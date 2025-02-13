namespace Linn.Stores2.Integration.Tests.StorageTypeModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private StorageType storageType;

        [SetUp]
        public void SetUp()
        {
            this.storageType = new StorageType("ST1", "StorageType 1 Description");

            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);

            this.Response = this.Client.Get(
                "/stores2/storage-types/ST1",
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
            var resource = this.Response.DeserializeBody<StorageTypeResource>();
            resource.StorageTypeCode.Should().Be("ST1");
            resource.Description.Should().Be("StorageType 1 Description");
        }
    }
}
