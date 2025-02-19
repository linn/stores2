namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBookingRequisition : ContextBase
    {
        private BookRequisitionResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new BookRequisitionResource
            {
                ReqNumber = 123
            };

            var req = new ReqWithReqNumber(
                11827635,
                new Employee(),
                TestFunctionCodes.Audit,
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
            req.Cancel("just cos", new Employee());
            this.ReqManager.BookRequisition(this.resource.ReqNumber, null, Arg.Any<int>(), Arg.Any<IEnumerable<string>>())
                .Returns(req);
            this.Response = this.Client.PostAsJsonAsync("/requisitions/book", this.resource).Result;
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.ReqManager.Received(1).BookRequisition(
                this.resource.ReqNumber, null, Arg.Any<int>(), Arg.Any<IEnumerable<string>>());
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
