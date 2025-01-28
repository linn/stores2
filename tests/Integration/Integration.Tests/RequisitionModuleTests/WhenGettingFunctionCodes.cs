namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NUnit.Framework;

    public class WhenGettingFunctionCodes : ContextBase
    {
        private StoresFunctionCode ldreq;

        [SetUp]
        public void SetUp()
        {
            this.ldreq = new StoresFunctionCode("LDREQ");

            this.DbContext.StoresFunctionCodes.AddAndSave(this.DbContext, this.ldreq);

            this.Response = this.Client.Get(
                "/requisitions/function-codes",
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
            var resource = this.Response.DeserializeBody<IEnumerable<FunctionCodeResource>>();
            resource.First().Id.Should().Be("LDREQ");
        }
    }
}
