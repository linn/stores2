namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NUnit.Framework;

    public class WhenSearchingByEmployee : ContextBase
    {
        private RequisitionHeader req123;

        private RequisitionHeader req456;

        [SetUp]
        public void SetUp()
        {
            var code = new StoresFunction { FunctionCode = "F1" };
            this.req123 = new RequisitionHeader(
                new Employee { Id = 123 },
                code,
                "F",
                12345678,
                "TYPE",
                new Department { DepartmentCode = "DEP1" },
                new Nominal { NominalCode = "NOM1" },
                reference: null,
                comments: "Emp 123 req");
            this.req456 = new RequisitionHeader(
                new Employee { Id = 456 },
                code,
                "F",
                12345678,
                "TYPE",
                new Department { DepartmentCode = "DEP2" },
                new Nominal { NominalCode = "NOM2" },
                reference: null,
                comments: "Emp 456 req");

            this.DbContext.RequisitionHeaders.AddAndSave(
                this.DbContext, this.req123);
            this.DbContext.RequisitionHeaders.AddAndSave(
                this.DbContext, this.req456);

            this.Response = this.Client.Get(
                "/requisitions?employeeId=456",
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
        public void ShouldOnlyReturnOneMatchingResult()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<RequisitionHeaderResource>>().ToList();
            resource.Should().HaveCount(1);
            resource.First().Comments.Should().Be("Emp 456 req");
        }
    }
}
