namespace Linn.Stores2.Integration.Tests.PcasStorageTypeModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Pcas;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PcasStorageTypeResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new PcasStorageTypeResource
                                      {
                                          BoardCode = "NEW-TEST-BOARD-CODE",
                                          StorageTypeCode = "NEW-TEST-STORAGE-TYPE-CODE",
                                          Maximum = 100,
                                          Incr = 1,
                                          Remarks = "NEW REMARKS",
                                          Preference = "1",
                                      };

            this.Response = this.Client.PostAsJsonAsync($"/stores2/pcas-storage-types", this.createResource).Result;
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
                .Remarks.Should().Be(this.createResource.Remarks);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<PcasStorageTypeResource>();
            resource.BoardCode.Should().Be("TEST BOARD CODE");
            resource.StorageTypeCode.Should().Be("TEST STORAGE TYPE CODE");
            resource.Remarks.Should().Be("A REMARKS");
        }
    }
}
