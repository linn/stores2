namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private ImportBookExchangeRateResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new ImportBookExchangeRateResource
            {
                PeriodNumber = 1234,
                BaseCurrency = TestCurrencies.UKPound.Code,
                ExchangeCurrencyCode = TestCurrencies.SwedishKrona.Code,
                ExchangeRate = 12.5m
            };

            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.UKPound);
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.SwedishKrona);

            this.Response = this.Client.PostAsJsonAsync(
                "/stores2/import-book-exchange-rates",
                this.createResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldPersistExchangeRate()
        {
            this.DbContext.ImportBookExchangeRates
                .Should().ContainSingle(r =>
                    r.PeriodNumber == this.createResource.PeriodNumber
                    && r.BaseCurrency == this.createResource.BaseCurrency
                    && r.ExchangeCurrencyCode == this.createResource.ExchangeCurrencyCode);
        }

        [Test]
        public void ShouldReturnCreatedResource()
        {
            var result = this.Response.DeserializeBody<ImportBookExchangeRateResource>();
            result.PeriodNumber.Should().Be(this.createResource.PeriodNumber);
            result.BaseCurrency.Should().Be(this.createResource.BaseCurrency);
            result.ExchangeCurrencyCode.Should().Be(this.createResource.ExchangeCurrencyCode);
            result.ExchangeRate.Should().Be(this.createResource.ExchangeRate);
        }
    }
}
