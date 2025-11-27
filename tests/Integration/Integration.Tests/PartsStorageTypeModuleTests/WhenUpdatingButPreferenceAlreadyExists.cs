namespace Linn.Stores2.Integration.Tests.PartsStorageTypeModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    using NUnit.Framework;

    public class WhenUpdatingButPreferenceAlreadyExists : ContextBase
    {
        private PartsStorageType partsStorageType1, partsStorageType2;

        private PartsStorageTypeResource updateResource;

        private Part part;

        private StorageType storageType1, storageType2;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part
            {
                Id = 1,
                PartNumber = "Part No 1",
                Description = "Part 1"
            };

            this.storageType1 = new StorageType { StorageTypeCode = "Storage Type No 1", };

            this.storageType2 = new StorageType { StorageTypeCode = "Storage Type No 2", };


            this.partsStorageType1 = new PartsStorageType(
                this.part,
                this.storageType1,
                "a",
                100,
                50,
                "1",
                400);

            this.partsStorageType2 = new PartsStorageType(
                this.part,
                this.storageType2,
                "a",
                100,
                50,
                "2",
                500);

            this.updateResource = new PartsStorageTypeResource
                                      {
                                          Part = new PartResource
                                                     {
                                                         PartNumber = this.part.PartNumber,
                                                         Description = this.part.Description,
                                                     },
                                          PartNumber = this.part.PartNumber,
                                          StorageType = new StorageTypeResource
                                                            {
                                                                StorageTypeCode = this.storageType2.StorageTypeCode,
                                                            },
                                          StorageTypeCode = this.storageType2.StorageTypeCode,
                                          Preference = "1",
                                          BridgeId = 500,
                                          Incr = 30,
                                          Maximum = 50,
                                          Remarks = "b"
                                      };

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partsStorageType1);

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partsStorageType2);

            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/parts-storage-types/{this.updateResource.BridgeId}",
                this.updateResource).Result;
        }

        [Test]
        public void ShouldReturnNotOk()
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
