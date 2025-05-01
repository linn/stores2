namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NUnit.Framework;

    public class WhenCreatingAndCodeAlreadyExists : ContextBase
    {
        private WorkstationResource createResource;

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

            this.createResource = new WorkstationResource
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

            this.Response = this.Client.PostAsJsonAsync($"/stores2/work-stations", this.createResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
