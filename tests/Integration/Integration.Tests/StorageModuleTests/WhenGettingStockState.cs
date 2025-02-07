namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingStockState : ContextBase
    {
        private StockState stockState;

        [SetUp]
        public void SetUp()
        {
            this.stockState = new StockState
            {
                State = "STORES",
                Description = "Good Stock",
                QCRequired = "N"
            };

            this.DbContext.StockStates.AddAndSave(this.DbContext, this.stockState);
            this.DbContext.SaveChanges();

            this.Response = this.Client.Get(
                "/stores2/stock/states/STORES",
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
            var resource = this.Response.DeserializeBody<StockStateResource>();
            resource.State.Should().Be(this.stockState.State);
            resource.Description.Should().Be(this.stockState.Description);
            resource.QCRequired.Should().Be(this.stockState.QCRequired);
        }
    }
}
