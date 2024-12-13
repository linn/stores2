namespace Linn.Stores2.Integration.Tests.CarrierModuleTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private CarrierResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new CarrierResource
                                      {
                                          Code = "DHL",
                                          Name = "D H L",
                                          Addressee = "MR Dhl",
                                          Line1 = "Line 1",
                                          CountryCode = "GB"
                                      };
            this.DbContext.Countries.AddAndSave(this.DbContext, new Country("GB", "Britain"));

            this.Response = this.Client.PostAsJsonAsync($"/stores2/carriers", this.createResource).Result;
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
            this.DbContext.Carriers
                .FirstOrDefault(x => x.CarrierCode == this.createResource.Code)
                .Name.Should().Be(this.createResource.Name);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<CarrierResource>();
            resource.Name.Should().Be(this.createResource.Name);
            resource.CountryCode.Should().Be("GB");
        }
    }
}
