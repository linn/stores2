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

    public class WhenUpdating : ContextBase
    {
        private Carrier dhl;

        private CarrierUpdateResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.dhl = new Carrier(
                "DHL",
                "D H L", 
                "Mr Dhl",
                "line2",
                "line2",
                "line3",
                "line4",
                "postcode",
                new Country("GB", "Great Britain"),
                "012345",
                "123456789");

            this.updateResource = new CarrierUpdateResource
                                      {
                                          Name = "DHL 2.0",
                                      };

            this.DbContext.Carriers.AddAndSave(this.DbContext, this.dhl);
            this.DbContext.SaveChanges();
            this.Response = this.Client.PutAsJsonAsync($"/stores2/carriers/{this.dhl.CarrierCode}", this.updateResource).Result;
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
            this.DbContext.Carriers
                .FirstOrDefault(x => x.CarrierCode == this.dhl.CarrierCode)
                .Name.Should().Be(this.updateResource.Name);
        }
        
        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<CarrierResource>();
            resource.Name.Should().Be(this.updateResource.Name);
        }
    }
}
