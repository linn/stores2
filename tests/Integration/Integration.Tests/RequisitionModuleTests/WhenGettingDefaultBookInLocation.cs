namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDefaultBookInLocation : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.StoresService.DefaultBookInLocation("PART123")
                .Returns(new StorageLocation { LocationCode = "LOC1", LocationId = 123 });
            this.Response = this.Client.Get(
                "/requisitions/default-book-in-location?partNumber=PART123",
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
            var resource = this.Response.DeserializeBody<StorageLocationResource>();
            resource.LocationId.Should().Be(123);
            resource.LocationCode.Should().Be("LOC1");
        }
    }
}
