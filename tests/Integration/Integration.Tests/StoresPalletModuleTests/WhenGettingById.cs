﻿namespace Linn.Stores2.Integration.Tests.StoresPalletModuleTests
{
    using System;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private StoresPallet pallet;

        private Employee employee;

        private StorageLocation storageLocation;

        [SetUp]
        public void SetUp()
        {
            this.storageLocation = new StorageLocation
                                       {
                                           LocationId = 3,
                                           Description = "Test Location"
                                       };

            this.employee = new Employee { Id = 123, Name = "Pallets Pat" };

            this.pallet = new StoresPallet(
                1,
                "Test-Description",
                this.storageLocation,
                3,
                "Y",
                "Y",
                1,
                "Y",
                1,
                null,
                null,
                123,
                null,
                null,
                "A",
                "A",
                4,
                1,
                "State1,State2",
                "A", 
                "Y");

            this.DbContext.StorageLocations.AddAndSave(this.DbContext, this.storageLocation);

            this.DbContext.Employees.AddAndSave(this.DbContext, this.employee);

            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet);

            this.Response = this.Client.Get(
                "/stores2/pallets/1",
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
            var resource = this.Response.DeserializeBody<StoresPalletResource>();
            resource.PalletNumber.Should().Be(1);
            resource.Description.Should().Be("Test-Description");
        }
    }
}

