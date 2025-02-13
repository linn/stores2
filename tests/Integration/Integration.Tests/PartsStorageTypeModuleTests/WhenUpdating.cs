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

    public class WhenUpdating : ContextBase
    {
        private PartsStorageType partsStorageType;

        private PartsStorageTypeResource updateResource;

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


            this.partsStorageType = new PartsStorageType(
                this.part,
                this.storageType,
                "a",
                100,
                50,
                "1",
                400);

            this.DbContext.Parts.AddAndSave(this.DbContext, this.part);

            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);

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
                                                                StorageTypeCode = this.storageType.StorageTypeCode,
                                                                Description = this.storageType.Description
                                                            },
                                          StorageTypeCode = this.storageType.StorageTypeCode,
                                          BridgeId = 4,
                                          Preference = "2",
                                          Incr = 30,
                                          Maximum = 50,
                                          Remarks = "b"
                                        };

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partsStorageType);
            this.Response = this.Client.PutAsJsonAsync($"/stores2/parts-storage-types/{this.part.PartNumber}/{this.storageType.StorageTypeCode}", this.updateResource).Result;
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
            this.DbContext.PartsStorageTypes.FirstOrDefault(x => x.PartNumber == this.partsStorageType.PartNumber && x.StorageTypeCode == this.partsStorageType.StorageTypeCode)
                .Remarks.Should().Be(this.updateResource.Remarks);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PartsStorageTypeResource>();
            resource.Remarks.Should().Be("b");
        }
    }
}
