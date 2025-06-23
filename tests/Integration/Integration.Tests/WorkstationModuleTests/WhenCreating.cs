namespace Linn.Stores2.Integration.Tests.WorkstationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private WorkstationResource createResource;
        private StorageLocation location;
        private StoresPallet pallet;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.WorkstationAdmin, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.location = new StorageLocation
            {
                LocationId = 1,
                LocationCode = "E-MH-KIT",
                StorageAreaCode = "MHK",
                Description = "MARK H"
            };

            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.location);
            this.DbContext.SaveChanges();

            this.pallet = new StoresPallet
            {
                PalletNumber = 999,
                Description = "PALLET 999"
            };

            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet);
            this.DbContext.SaveChanges();

            this.createResource = new WorkstationResource
            {
                WorkStationCode = "WORKSTATIONCODE",
                CitCode = "R",
                CitName = "R CODE",
                Description = "A TEST WORKSTATION",
                ZoneType = "Z",
                WorkStationElements = new List<WorkstationElementResource>
                                                                    {
                                                                        new WorkstationElementResource()
                                                                            {
                                                                                WorkstationCode = "WORKSTATIONCODE",
                                                                                CreatedById = 33156,
                                                                                CreatedByName = "RSTEWART",
                                                                                DateCreated = DateTime.Today.ToString("o"),
                                                                                LocationId = this.location.LocationId,
                                                                                WorkStationElementId = 1
                                                                            },
                                                                        new WorkstationElementResource()
                                                                            {
                                                                                WorkstationCode = "WORKSTATIONCODE",
                                                                                CreatedById = 33156,
                                                                                CreatedByName = "RSTEWART",
                                                                                DateCreated = DateTime.Today.ToString("o"),
                                                                                PalletNumber = this.pallet.PalletNumber,
                                                                                WorkStationElementId = 2
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
                .First(x => x.WorkStationCode == this.createResource.WorkStationCode).Description
                .Should().Be(this.createResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<WorkstationResource>();
            resource.WorkStationCode.Should().Be("WORKSTATIONCODE");
            resource.Description.Should().Be("A TEST WORKSTATION");
        }
    }
}
