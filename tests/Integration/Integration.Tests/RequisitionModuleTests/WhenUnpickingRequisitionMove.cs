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

    public class WhenUnpickingRequisitionMove : ContextBase
    {
        private UnpickRequisitionResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new UnpickRequisitionResource
            {
                ReqNumber = 1,
                LineNumber = 1,
                Seq = 1,
                QtyToUnpick = 1,
                Reallocate = false
            };

            var req = new ReqWithReqNumber(
                1,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
           
            this.ReqManager.UnpickRequisitionMove(1, 1,1,1, Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<IEnumerable<string>>())
                .Returns(req);
            this.Response = this.Client.PostAsJsonAsync("/requisitions/unpick", this.resource).Result;
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.ReqManager.Received(1).UnpickRequisitionMove(1, 1, 1, 1, Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<IEnumerable<string>>());
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
    }
}
