namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenGettingReversalPreview : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC", CanBeReversed = "Y" },
                "F",
                123,
                "REQ",
                new Department { DepartmentCode = "DEPT" },
                new Nominal { NominalCode = "NOM" });

            this.DbContext.RequisitionHeaders.AddAndSave(this.DbContext, this.req);

            this.Response = this.Client.Get(
                "/requisitions/123/preview-reversal",
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
            resource.OriginalReqNumber.Should().Be(123);
        }
    }
}

