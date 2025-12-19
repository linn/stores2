namespace Linn.Stores2.Integration.Tests.PartStorageTypeModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;

    using NUnit.Framework;

    public class WhenDeleting : ContextBase
    {
        private PartStorageType partStorageType;

        private Part part;

        private StorageType storageType;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part { Id = 1, PartNumber = "Part No 1", Description = "Part 1" };

            this.storageType = new StorageType
                                   {
                                       StorageTypeCode = "Storage Type No 1",
                                       Description = "Storage Type 1"
                                   };


            this.partStorageType = new PartStorageType(
                this.part,
                this.storageType,
                "a",
                100,
                50,
                "1",
                400);

            this.DbContext.PartsStorageTypes.AddAndSave(this.DbContext, this.partStorageType);

            this.Response = this.Client.Delete(
                "/stores2/part-storage-types/400",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOK()
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
        public void ShouldBeEmpty()
        {
            this.DbContext.PartsStorageTypes
                .Any(x => x.PartNumber == this.partStorageType.PartNumber && x.StorageTypeCode == this.partStorageType.StorageTypeCode)
                .Should().BeFalse();
        }
    }
}