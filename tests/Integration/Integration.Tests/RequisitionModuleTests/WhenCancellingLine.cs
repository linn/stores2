namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingLine : ContextBase
    {
        private CancelRequisitionResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new CancelRequisitionResource
            {
                Reason = "Just cos",
                ReqNumber = 123,
                LineNumber = 1
            };
            var req = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
            req.Cancel("reasonable reason", new Employee());
            this.ReqManager.CancelLine(
                    this.resource.ReqNumber,
                    this.resource.LineNumber.Value,
                    Arg.Any<int>(),
                    Arg.Any<IEnumerable<string>>(),
                    this.resource.Reason)
                .Returns(req);
            this.Response = this.Client.PostAsJsonAsync("/requisitions/cancel", this.resource).Result;
        }

        [Test]
        public void ShouldCancelLine()
        {
            this.ReqManager.Received(1).CancelLine(
                this.resource.ReqNumber, 
                this.resource.LineNumber.Value,
                Arg.Any<int>(), 
                Arg.Any<IEnumerable<string>>(),
                this.resource.Reason);
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
