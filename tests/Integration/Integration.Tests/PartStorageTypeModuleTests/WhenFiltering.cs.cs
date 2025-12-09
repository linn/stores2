namespace Linn.Stores2.Integration.Tests.PartsStorageTypeModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Parts;

    using NUnit.Framework;

    public class WhenFiltering : ContextBase
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

            this.Response = this.Client.Get(
                $"/stores2/part-storage-types?part={this.part.PartNumber}&storageType={this.storageType.StorageTypeCode}",
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
            var resource = this.Response.DeserializeBody<IEnumerable<PartStorageTypeResource>>().ToList();
            resource.First().StorageTypeCode.Should().Be("Storage Type No 1");
            resource.First().PartNumber.Should().Be("Part No 1");
            resource.First().BridgeId.Should().Be(400);
        }
    }
}
