namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenCreatingStockState : ContextBase
    {
        private StockStateResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new StockStateResource
            {
                State = "QC",
                Description = "Quality Control",
                QCRequired = "Y"
            };

            this.Response = this.Client.PostAsJsonAsync("/stores2/stock/states", this.createResource).Result;
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
        public void ShouldPersistStockState()
        {
            this.DbContext.StockStates
                .First(x => x.State == this.createResource.State)
                .Description.Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnCreatedResource()
        {
            var resource = this.Response.DeserializeBody<StockStateResource>();
            resource.State.Should().Be(this.createResource.State);
            resource.Description.Should().Be(this.createResource.Description);
            resource.QCRequired.Should().Be(this.createResource.QCRequired);
        }
    }
}
