﻿namespace Linn.Stores2.Integration.Tests.WorkStationModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;

    using NUnit.Framework;

    public class WhenUpdatingWithNoStorageLocationOrPallet : ContextBase
    {
        private WorkStation workStation;

        private WorkStationResource updateResource;

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
                                                      DateCreated = DateTime.Today.ToString("o")
                                                  },
                                              new WorkStationElementResource
                                                  {
                                                      WorkStationCode = "Test",
                                                      CreatedById = 33156,
                                                      CreatedByName = "RSTEWART",
                                                      DateCreated = DateTime.Today.ToString("o")
                                                  }
                                          }
            };

            this.DbContext.WorkStations.AddAndSave(this.DbContext, this.workStation);
            this.DbContext.SaveChanges();

            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/work-stations/Test",
                this.updateResource).Result;
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldThrow()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
