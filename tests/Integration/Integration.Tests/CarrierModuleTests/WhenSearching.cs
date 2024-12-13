namespace Linn.Stores2.Integration.Tests.CarrierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private Carrier dhl;
        
        private Carrier fedex;
        
        [SetUp]
        public void SetUp()
        {
            this.dhl = new Carrier(
                "DHL",
                "DHL Logistics", 
                "Mr Dhl",
                "line2",
                "line2",
                "line3",
                "line4",
                "postcode",
                new Country("GB", "Great Britain"),
                "012345",
                "123456789");
            
            this.fedex = new Carrier(
                "FDX",
                "Fedex", 
                "Mr Fedex",
                "line2",
                "line2",
                "line3",
                "line4",
                "postcode",
                new Country("IE", "Ireland"),
                "01225556",
                "987654321");

            this.DbContext.Carriers.AddAndSave(this.DbContext, this.dhl);
            this.DbContext.Carriers.AddAndSave(this.DbContext, this.fedex);
            
            this.Response = this.Client.Get(
                "/stores2/carriers?searchTerm=dhl",
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
        
        // This wouldn't have been possible before, since IRepository's were mocked out
        // and so we couldn't test whether the correct Expression<Func<Carrier, bool>> filterExpression was passed to FilterBy
        // and subsequently applied correctly
        [Test]
        public void ShouldReturnCorrectlyFilteredResults()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<CarrierResource>>();
            resource.Count().Should().Be(1);
            resource.First().Code.Should().Be("DHL");
        }
    }
}
