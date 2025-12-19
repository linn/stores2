namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Pcas;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PcasStorageType createPcasStorageType;

        private PcasStorageTypeResource createResource;

        private StorageType storageType;

        private PcasBoard pcasBoard;

        [SetUp]
        public void SetUp()
        {
            this.storageType = new StorageType
                                   {
                                       StorageTypeCode = "TEST-STORAGE-TYPE-CODE",
                                       Description = "Storage Type Description"
                                   };

            this.pcasBoard = new PcasBoard
                                 {
                                     BoardCode = "TEST-BOARD-CODE",
                                     Description = "PCAS Board Description"
                                 };
            this.createResource = new PcasStorageTypeResource
                                      {
                                          BoardCode = "TEST-BOARD-CODE",
                                          StorageTypeCode = "TEST-STORAGE-TYPE-CODE",
                                          Maximum = 100,
                                          Increment = 1,
                                          Remarks = "A REMARKS",
                                          Preference = "1",
                                      };

            this.createPcasStorageType = new PcasStorageType(
                this.pcasBoard,
                this.storageType,
                100,
                1,
                "A REMARKS",
                "2");

            this.DbContext.PcasBoards.AddAndSave(this.DbContext, this.pcasBoard);
            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);

            this.StorageTypeService.ValidateCreatePcasStorageType(Arg.Any<PcasStorageType>())
                .Returns(this.createPcasStorageType);

            this.Response = this.Client.PostAsJsonAsync("/stores2/pcas-storage-types", this.createResource).Result;
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
            this.DbContext.PcasStorageTypes
                .FirstOrDefault(x => x.StorageTypeCode == this.createResource.StorageTypeCode && x.BoardCode == this.createResource.BoardCode)
                ?.Remarks.Should().Be(this.createResource.Remarks);
        }
        
        [Test]
        public void ShouldReturnCreatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PcasStorageTypeResource>();
            resource.BoardCode.Should().Be("TEST-BOARD-CODE");
            resource.StorageTypeCode.Should().Be("TEST-STORAGE-TYPE-CODE");
            resource.Remarks.Should().Be("A REMARKS");
        }
    }
}
