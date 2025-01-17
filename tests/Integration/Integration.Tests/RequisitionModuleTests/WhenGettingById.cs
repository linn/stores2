namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new RequisitionHeader(123, "Hello Requisitions");

            this.DbContext.RequisitionHeaders.AddAndSave(this.DbContext, this.req);

            this.Response = this.Client.Get(
                "/requisitions/123",
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
            var resource = this.Response.DeserializeBody<RequisitionHeaderResource>();
            resource.Comments.Should().Be("Hello Requisitions");
        }
    }
}

