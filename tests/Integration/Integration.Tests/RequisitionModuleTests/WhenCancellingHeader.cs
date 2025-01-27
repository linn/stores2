namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingHeader : ContextBase
    {
        private CancelRequisitionResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new CancelRequisitionResource
                                      {
                                          Reason = "Just cos",
                                          ReqNumber = 123
                                      };
            this.DomainService.CancelHeader(this.resource.ReqNumber, Arg.Any<User>(), this.resource.Reason)
                .Returns(new CancelledRequisitionHeader(this.resource.ReqNumber));
            this.Response = this.Client.PostAsJsonAsync("/requisitions/cancel", this.resource).Result;
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.DomainService.Received(1).CancelHeader(
                this.resource.ReqNumber, Arg.Any<User>(), this.resource.Reason);
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
        public void ShouldReturnCancelled()
        {
            var result = this.Response.DeserializeBody<RequisitionHeaderResource>();
            result.DateCancelled.Should().NotBe(null);
        }
    }
}
