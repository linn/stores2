namespace Linn.Stores2.Integration.Tests.PartsStorageTypeModuleTests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PartsStorageType partsStorageType;

        private Part part;

        private StorageType storageType;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part
            {
                Id = 1,
                PartNumber = "Part No 1",
                Description = "Part 1"
            };

            this.storageType = new StorageType { StorageTypeCode = "Storage Type No 1", };

            this.DbContext.Parts.AddAndSave(this.DbContext, this.part);

            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);


            this.partsStorageType = new PartsStorageType(
                new Part
                    {
                        Id = 1,
                        PartNumber = "Part No 1",
                        Description = "Part 1"
                    },
                new StorageType { StorageTypeCode = "Storage Type No 1", },
                "a",
                100,
                50,
                "1",
                400);

            this.Response = this.Client.PostAsJsonAsync($"/stores2/parts-storage-types", this.partsStorageType).Result;
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
            this.DbContext.PartsStorageTypes
                .FirstOrDefault(x => x.PartNumber == this.partsStorageType.PartNumber && x.StorageTypeCode == this.partsStorageType.StorageTypeCode)
                .Remarks.Should().Be(this.partsStorageType.Remarks);
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PartsStorageTypeResource>();
            resource.PartNumber.Should().Be("Part No 1");
            resource.Incr.Should().Be(50);
        }
    }
}
