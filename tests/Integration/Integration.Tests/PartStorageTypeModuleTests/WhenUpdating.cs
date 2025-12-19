namespace Linn.Stores2.Integration.Tests.PartStorageTypeModuleTests
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

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PartStorageType partsStorageType, updatedPartsStorageType;

        private PartStorageTypeResource updateResource;

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


            this.partsStorageType = new PartStorageType(
                this.part,
                this.storageType,
                "a",
                100,
                50,
                "1",
                400);

            this.updateResource = new PartStorageTypeResource
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
                                          Preference = "2",
                                          BridgeId = 400,
                                          Incr = 30,
                                          Maximum = 50,
                                          Remarks = "b"
                                        };

            this.updatedPartsStorageType = new PartStorageType(
                this.part,
                this.storageType,
                "b",
                100,
                30, 
                "2",
                400);

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partsStorageType);

            this.StorageTypeService.ValidatePartsStorageType(Arg.Any<PartStorageType>()).Returns(this.updatedPartsStorageType);

            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/part-storage-types/{this.partsStorageType.BridgeId}",
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
            this.DbContext.PartsStorageTypes.First(
                    x => x.PartNumber == this.partsStorageType.PartNumber
                         && x.StorageTypeCode == this.partsStorageType.StorageTypeCode)
                .Remarks.Should().Be(this.updatedPartsStorageType.Remarks);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PartStorageTypeResource>();
            resource.Remarks.Should().Be(this.updatedPartsStorageType.Remarks);
        }
    }
}