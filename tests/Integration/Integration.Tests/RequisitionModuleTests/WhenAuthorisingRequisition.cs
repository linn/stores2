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
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorisingRequisition : ContextBase
    {
        private AuthoriseRequisitionResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new AuthoriseRequisitionResource
            {
                ReqNumber = 123
            };

            var authorisedBy = new Employee
            {
                Id = 1, Name = "Thomas The Tank"
            };

            var req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
            req.Lines.Add(new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLinnDept));
            req.Authorise(authorisedBy);

            this.ReqManager.AuthoriseRequisition(
                    this.resource.ReqNumber,
                    Arg.Any<int>(),
                    Arg.Any<IEnumerable<string>>())
                .Returns(req);
            this.Response = this.Client.PostAsJsonAsync("/requisitions/authorise", this.resource).Result;
        }

        [Test]
        public void ShouldAuthoriseHeader()
        {
            this.ReqManager.Received(1).AuthoriseRequisition(
                this.resource.ReqNumber, Arg.Any<int>(), Arg.Any<IEnumerable<string>>());
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
        public void ShouldReturnAuthorisedReq()
        {
            var result = this.Response.DeserializeBody<RequisitionHeaderResource>();
            result.DateAuthorised.Should().NotBe(null);
            result.AuthorisedBy.Should().Be(1);
        }
    }
}
