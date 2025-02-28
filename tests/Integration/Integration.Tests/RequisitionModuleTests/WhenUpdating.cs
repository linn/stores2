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
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private RequisitionHeaderResource resource;
        private RequisitionHeader req;

        [SetUp]
        public void Setup()
        {
            this.resource = new RequisitionHeaderResource
            {
                CreatedBy = 33087,
                StoresFunction = new StoresFunctionResource { Code = "FUNC" },
                ReqType = "F",
                Department = new DepartmentResource { DepartmentCode = "1607" },
                Nominal = new NominalResource { NominalCode = "2963" },
                Lines = new List<RequisitionLineResource>(),
                FromStockPool = "LINN"
            };
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department { DepartmentCode = "DEPT" },
                new Nominal { NominalCode = "NOM" });

            this.DbContext.RequisitionHeaders.AddAndSave(this.DbContext, this.req);

            this.Response = this.Client.PostAsJsonAsync("/requisitions/123", this.resource).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldSaveLog()
        {
            this.RequisitionHistoryRepository.Received().AddAsync(Arg.Any<RequisitionHistory>());
        }
        
        [Test]
        public void ShouldCallDomain()
        {
            this.ReqManager.Received().UpdateRequisition(
                Arg.Any<RequisitionHeader>(), Arg.Any<IEnumerable<LineCandidate>>());
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
            resource.ReqNumber.Should().Be(123);
        }
    }
}
