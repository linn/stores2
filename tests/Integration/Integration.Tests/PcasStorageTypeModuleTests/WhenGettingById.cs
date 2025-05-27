namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Pcas;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private PcasStorageType pcasStorageType;

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

            this.pcasStorageType = new PcasStorageType(
                this.pcasBoard,
                this.storageType,
                2,
                1,
                "REMARKS",
                "1");

            this.DbContext.PcasBoards.AddAndSave(this.DbContext, this.pcasBoard);
            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);
            this.DbContext.PcasStorageTypes.AddAndSave(this.DbContext, this.pcasStorageType);

            this.Response = this.Client.Get(
                $"/stores2/pcas-storage-types/{this.pcasStorageType.BoardCode}/{this.pcasStorageType.StorageTypeCode}",
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
            var resource = this.Response.DeserializeBody<PcasStorageTypeResource>();
            resource.BoardCode.Should().Be("TEST-BOARD-CODE");
            resource.StorageTypeCode.Should().Be("TEST-STORAGE-TYPE-CODE");
            resource.Increment.Should().Be(1);
        }
    }
}
