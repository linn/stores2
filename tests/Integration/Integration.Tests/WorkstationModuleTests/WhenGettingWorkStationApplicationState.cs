namespace Linn.Stores2.Integration.Tests.WorkStationModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingApplicationState : ContextBase
    {
        [SetUp] 
        public void SetUp()
        {
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.WorkStationAdmin, Arg.Any<List<string>>())
                .Returns(true);

            this.Response = this.Client.Get(
                "/stores2/work-stations/application-state",
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
            var resource = this.Response.DeserializeBody<WorkStationResource>();
            resource.Links.Count(x => x.Rel == "create").Should().Be(1);
        }
    }
}
