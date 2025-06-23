namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Stores;
    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private Workstation workstation;
        private Workstation secondWorkstation;

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
                "Z",
                new List<WorkstationElement>
                    {
                    });

            this.secondWorkstation = new Workstation(
                "aredundantworkstation",
                "description",
                new Cit
                    {
                        Code = "S",
                        Name = "S CODE"
                    },
                "D",
                new List<WorkstationElement>
                    {
                    });

            this.DbContext.Workstations.AddAndSave(this.DbContext, this.workstation);
            this.DbContext.Workstations.AddAndSave(this.DbContext, this.secondWorkstation);


            this.Response = this.Client.Get(
                "/stores2/work-stations?workstationCode=Test",
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
            var resources = this.Response.DeserializeBody<IEnumerable<WorkstationResource>>().ToList();
            resources.Count.Should().Be(1);
            resources.First().WorkStationCode.Should().Be("Test");
            resources.First().Description.Should().Be("description");
        }
    }
}