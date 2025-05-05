namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Pcas;


    using NUnit.Framework;

    public class WhenGettingAll : ContextBase
    {
        private PcasStorageType pcasStorageTypes;

        [SetUp]
        public void SetUp()
        {
            this.pcasStorageTypes = new PcasStorageType(
                "TEST-BOARD-CODE",
                "TEST-STORAGE-TYPE-CODE",
                2,
                1,
                "REMARKS",
                "1");

            this.DbContext.PcasStorageTypes.AddAndSave(this.DbContext, this.pcasStorageTypes);

            this.Response = this.Client.Get(
                "/stores2/pcas-storage-types",
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
            var resource = this.Response.DeserializeBody<IEnumerable<PcasStorageTypeResource>>().ToList();
            resource.First().BoardCode.Should().Be("TEST-BOARD-CODE");
            resource.First().StorageTypeCode.Should().Be("TEST-STORAGE-TYPE-CODE");
            resource.First().Preference.Should().Be("1");
        }
    }
}
