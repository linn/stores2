namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenUpdatingStockState : ContextBase
    {
        private StockState existing;

        private StockStateResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.existing = new StockState("STORES", "Good Stock")
                                {
                                    QCRequired = "N"
                                };

            this.DbContext.StockStates.AddAndSave(this.DbContext, this.existing);
            this.DbContext.SaveChanges();

            this.updateResource = new StockStateResource
            {
                State = "STORES",
                Description = "Updated Good Stock",
                QCRequired = "Y"
            };

            this.Response = this.Client
                .PutAsJsonAsync("/stores2/stock/states/STORES", this.updateResource).Result;
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
        public void ShouldUpdateDescription()
        {
            this.DbContext.StockStates
                .First(x => x.State == this.existing.State)
                .Description.Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedResource()
        {
            var resource = this.Response.DeserializeBody<StockStateResource>();
            resource.State.Should().Be(this.updateResource.State);
            resource.Description.Should().Be(this.updateResource.Description);
            resource.QCRequired.Should().Be(this.updateResource.QCRequired);
        }
    }
}
