﻿namespace Linn.Stores2.Integration.Tests.WorkStationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private WorkStation workstation;
        private StorageLocation location;
        private StoresPallet pallet;

        private WorkStationResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.WorkStationAdmin, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.location = new StorageLocation
            {
                LocationId = 1,
                LocationCode = "E-MH-KIT",
                StorageAreaCode = "MHK",
                Description = "MARK H"
            };

            this.pallet = new StoresPallet
            {
                PalletNumber = 999,
                Description = "PALLET 999"
            };

            this.workstation = new WorkStation(
                "Test",
                "description",
                new Cit
                {
                    Code = "R",
                    Name = "R CODE"
                },
                "Z",
                null);

            this.updateResource = new WorkStationResource
            {
                WorkStationCode = "Test",
                CitCode = "R",
                CitName = "R CODE",
                Description = "A TEST WORKSTATION",
                ZoneType = "Z",
                WorkStationElements = new List<WorkStationElementResource>
                                          {
                                              new WorkStationElementResource
                                                  {
                                                      WorkStationCode = "Test",
                                                      CreatedById = 33156,
                                                      CreatedByName = "RSTEWART",
                                                      DateCreated = DateTime.Today.ToString("o"),
                                                      LocationId = this.location.LocationId,
                                                      PalletNumber = 567
                                                  },
                                              new WorkStationElementResource
                                                  {
                                                      WorkStationCode = "Test",
                                                      CreatedById = 33156,
                                                      CreatedByName = "RSTEWART",
                                                      DateCreated = DateTime.Today.ToString("o"),
                                                      LocationId = 567,
                                                      PalletNumber = this.pallet.PalletNumber
                                                  }
                                          }
            };

            this.DbContext.WorkStations.AddAndSave(this.DbContext, this.workstation);
            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.location);
            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet);
            this.DbContext.SaveChanges();

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
            this.DbContext.WorkStations.First(x => x.WorkStationCode == this.workstation.WorkStationCode).Description
                .Should().Be(this.updateResource.Description);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<WorkStationResource>();
            resource.Description.Should().Be("A TEST WORKSTATION");
            resource.WorkStationElements.Count().Should().Be(2);
        }
    }
}
