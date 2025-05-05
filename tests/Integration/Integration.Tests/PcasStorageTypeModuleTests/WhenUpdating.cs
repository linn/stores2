namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Pcas;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PcasStorageType pcasStorageType;

        private PcasStorageTypeResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.pcasStorageType = new PcasStorageType(
                "TEST-BOARD-CODE",
                "TEST-STORAGE-TYPE-CODE",
                2,
                1,
                "REMARKS",
                "1");



            this.updateResource = new PcasStorageTypeResource
            {
                                          BoardCode = "TEST-BOARD-CODE",
                                          StorageTypeCode = "TEST-STORAGE-TYPE-CODE",
                                          Maximum = 100,
                                          Incr = 1,
                                          Remarks = "NEW REMARKS",
                                          Preference = "1",
                                      };

            this.DbContext.PcasStorageTypes.AddAndSave(this.DbContext, this.pcasStorageType);
            this.Response = this.Client.PutAsJsonAsync(
                    $"/stores2/pcas-storage-type/{this.pcasStorageType.BoardCode}/{this.pcasStorageType.StorageTypeCode}", this.updateResource).Result;
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
            this.DbContext.PcasStorageTypes
                .First(x => x.BoardCode == this.pcasStorageType.BoardCode && x.StorageTypeCode == this.pcasStorageType.StorageTypeCode).Remarks
                .Should().Be(this.updateResource.Remarks);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PcasStorageTypeResource>();
            resource.BoardCode.Should().Be("TEST-BOARD-CODE");
            resource.StorageTypeCode.Should().Be("TEST-STORAGE-TYPE-CODE");
            resource.Remarks.Should().Be("NEW REMARKS");
        }
    }
}
