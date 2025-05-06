namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Stores;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private WorkstationResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new WorkstationResource 
                                      {
                                          WorkstationCode = "WORKSTATIONCODE",
                                          CitCode = "R",
                                          CitName = "R CODE",
                                          Description = "A TEST WORKSTATION",
                                          ZoneType = "Z",
                                          WorkstationElements = new List<WorkstationElementResource>
                                                                    {
                                                                        new WorkstationElementResource()
                                                                            {
                                                                                WorkstationCode = "WORKSTATIONCODE",
                                                                                CreatedBy = 33156,
                                                                                CreatedByName = "RSTEWART",
                                                                                DateCreated = DateTime.Today.ToString("o"),
                                                                                LocationId = 123,
                                                                                PalletNumber = 567,
                                                                                WorkstationElementId = 1
                                                                            },
                                                                        new WorkstationElementResource()
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
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldAdd()
        {
            this.DbContext.Workstations
                .First(x => x.WorkstationCode == this.createResource.WorkstationCode).Description
                .Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<WorkstationResource>();
            resource.WorkstationCode.Should().Be("WORKSTATIONCODE");
            resource.Description.Should().Be("A TEST WORKSTATION");
        }
    }
}
