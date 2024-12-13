namespace Linn.Stores2.Integration.Tests.CarrierModuleTests
{
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private Carrier dhl;

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

            this.DbContext.Carriers.AddAndSave(this.DbContext, this.dhl);

            this.Response = this.Client.Get(
                "/stores2/carriers/DHL",
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
            var resource = this.Response.DeserializeBody<CarrierResource>();
            resource.Code.Should().Be("DHL");
        }
    }
}
