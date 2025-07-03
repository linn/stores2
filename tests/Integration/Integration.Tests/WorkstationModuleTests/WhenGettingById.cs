namespace Linn.Stores2.Integration.Tests.WorkStationModuleTests
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
        private WorkStation workStation;

        [SetUp]
        public void SetUp()
        {
            this.workStation = new WorkStation(
                "Test", 
                "description", 
                new Cit
                    {
                        Code = "R",
                        Name = "R CODE"
                    }, 
                "Z", 
                new List<WorkStationElement> 
                {
                });

            this.DbContext.WorkStations.AddAndSave(this.DbContext, this.workStation);

            this.Response = this.Client.Get(
                "/stores2/work-stations/Test",
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
            var resource = this.Response.DeserializeBody<WorkStationResource>();
            resource.WorkStationCode.Should().Be("Test");
            resource.Description.Should().Be("description");
        }
    }
}
