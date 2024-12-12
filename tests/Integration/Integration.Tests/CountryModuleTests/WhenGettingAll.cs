namespace Linn.Stores2.Integration.Tests.CountryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenGettingCountries : ContextBase
    {
        private Country greatBritain;

        [SetUp]
        public void SetUp()
        {
            this.greatBritain = new Country("GB", "Starmer's Britain");

            this.DbContext.Countries.Add(this.greatBritain);
            this.DbContext.SaveChanges();

            this.Response = this.Client.Get(
                "/stores2/countries",
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
            var resource = this.Response.DeserializeBody<IEnumerable<CountryResource>>();
            resource.First().CountryCode.Should().Be("GB");
        }
    }
}
