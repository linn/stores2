namespace Linn.Stores2.Integration.Tests.PartsStorageTypeModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingButStorageTypeAlreadyExists : ContextBase
    {
        private PartsStorageType partsStorageType, newPartsStorageType;

        private Part part;

        private StorageType storageType;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part { Id = 1, PartNumber = "Part No 1", Description = "Part 1" };

            this.storageType = new StorageType { StorageTypeCode = "Storage Type No 1", };

            this.partsStorageType = new PartsStorageType(this.part, this.storageType, "a", 100, 50, "1", 1);

            this.newPartsStorageType = new PartsStorageType(this.part, this.storageType, "a", 100, 50, "2", 400);

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partsStorageType);

            this.DatabaseService.GetNextVal("PARTS_STORAGE_TYPES_ID_SEQ").Returns(2);

            this.Response = this.Client.PostAsJsonAsync($"/stores2/parts-storage-types", this.newPartsStorageType).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }
    }
}
