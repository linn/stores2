namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private Workstation workstation;

        [SetUp]
        public void SetUp()
        {
            this.workstation = new Workstation(
                "Test", 
                "description", 
                new Cit
                    {
                        Code = "R",
                        Name = "R CODE"
                    }, 
                "V", 
                "Z", 
                new List<WorkstationElement> 
                {
                });

            this.DbContext.Workstations.AddAndSave(this.DbContext, this.workstation);

            this.Response = this.Client.Get(
                "/stores2/work-stations/test",
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
            var resource = this.Response.DeserializeBody<WorkstationResource>();
            resource.WorkstationCode.Should().Be("test");
            resource.Description.Should().Be("description");
        }
    }
}
