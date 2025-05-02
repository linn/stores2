namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private Workstation workstation;

        private WorkstationResource updateResource;

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

            this.updateResource = new WorkstationResource
            {
                WorkstationCode = "Test",
                CitCode = "R",
                CitName = "R CODE",
                Description = "A TEST WORKSTATION",
                ZoneType = "Z",
                VaxWorkstation = "161",
                WorkstationElements = new List<WorkstationElementResource>
                                          {
                                              new WorkstationElementResource
                                                  {
                                                      WorkstationCode = "WORKSTATIONCODE",
                                                      CreatedBy = 33156,
                                                      CreatedByName = "RSTEWART",
                                                      DateCreated = DateTime.Today.ToString("o"),
                                                      LocationId = 123,
                                                      PalletNumber = 567,
                                                      WorkstationElementId = 1
                                                  },
                                              new WorkstationElementResource
                                                  {
                                                      WorkstationCode = "WORKSTATIONCODE",
                                                      CreatedBy = 33156,
                                                      CreatedByName = "RSTEWART",
                                                      DateCreated = DateTime.Today.ToString("o"),
                                                      LocationId = 567,
                                                      PalletNumber = 890,
                                                      WorkstationElementId = 2
                                                  }
                                          }
            };

            this.DbContext.Workstations.AddAndSave(this.DbContext, this.workstation);
            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/work-stations/Test",
                this.updateResource).Result;
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
        public void ShouldUpdateEntity()
        {
            this.DbContext.Workstations.First(x => x.WorkstationCode == this.workstation.WorkstationCode).Description
                .Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<Workstation>();
            resource.Description.Should().Be("A TEST WORKSTATION");
        }
    }
}
