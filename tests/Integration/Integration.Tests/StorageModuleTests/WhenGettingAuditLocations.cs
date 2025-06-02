namespace Linn.Stores2.Integration.Tests.StorageModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAuditLocations : ContextBase
    {
        private AuditLocation location1;
        private AuditLocation location2;
        private AuditLocation location3;

        [SetUp]
        public void SetUp()
        {
            this.location1 = new AuditLocation { StoragePlace = "P1" };
            this.location2 = new AuditLocation { StoragePlace = "E-K1-2" };
            this.location3 = new AuditLocation { StoragePlace = "E-SOMEWHERE" };

            this.AuditLocationRepository.FindAllAsync()
                .Returns(new List<AuditLocation> { this.location1, this.location2, this.location3 });

            this.Response = this.Client.Get(
                "/stores2/storage/audit-locations",
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
            var resources = this.Response.DeserializeBody<IEnumerable<AuditLocationResource>>().ToList();
            resources.Should().HaveCount(3);
            resources.Should().Contain(a => a.StoragePlace == this.location1.StoragePlace);
            resources.Should().Contain(a => a.StoragePlace == this.location2.StoragePlace);
            resources.Should().Contain(a => a.StoragePlace == this.location3.StoragePlace);
        }
    }
}
