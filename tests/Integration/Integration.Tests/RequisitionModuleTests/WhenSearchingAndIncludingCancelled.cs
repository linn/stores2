namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NUnit.Framework;

    public class WhenSearchingAndIncludingCancelled : ContextBase
    {
        private RequisitionHeader req123;

        private RequisitionHeader req456;

        [SetUp]
        public void SetUp()
        {
            this.req123 = new RequisitionHeader(123, "Hello Requisitions");
            this.req123.Cancel(33087);
            this.req456 = new RequisitionHeader(456, "Goodbye Requisitions");

            this.DbContext.RequisitionHeaders.AddAndSave(this.DbContext, this.req123);
            this.DbContext.RequisitionHeaders.AddAndSave(this.DbContext, this.req456);

            this.Response = this.Client.Get(
                "/stores2/requisitions?comments=Req&includeCancelled=True",
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
        public void ShouldReturnAll()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<RequisitionHeaderResource>>();
            resource.Should().NotBeNull();
            resource.Count().Should().Be(2);
        }
    }
}
