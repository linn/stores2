namespace Linn.Stores2.Integration.Tests.RequisitionModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingRequisitionApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.ReverseRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Response = this.Client.Get(
                "/requisitions/application-state",
                with =>
                {
                    with.Accept("application/vnd.linn.application-state+json");
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
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/vnd.linn.application-state+json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<RequisitionHeaderResource>();
            resource.Links.Should().Contain(a => a.Rel == "create-reverse" && a.Href == "/requisitions/create");
        }
    }
}
