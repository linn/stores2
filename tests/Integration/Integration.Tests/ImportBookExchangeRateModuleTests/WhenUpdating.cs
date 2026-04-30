namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private ImportBookExchangeRate existingRate;

        [SetUp]
        public void SetUp()
        {
            this.existingRate = new ImportBookExchangeRate
            {
                PeriodNumber = 1234,
                BaseCurrency = TestCurrencies.UKPound.Code,
                ExchangeCurrencyCode = TestCurrencies.SwedishKrona.Code,
                ExchangeCurrency = TestCurrencies.SwedishKrona,
                ExchangeRate = 12.5m
            };

            this.DbContext.LedgerPeriods.AddAndSave(this.DbContext, new LedgerPeriod { PeriodNumber = 1234, MonthName = "January" });
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.SwedishKrona);
            this.DbContext.ImportBookExchangeRates.AddAndSave(this.DbContext, this.existingRate);

            var updateResource = new ImportBookExchangeRateResource { ExchangeRate = 13.75m };

            this.Response = this.Client.PutAsJsonAsync(
                $"/stores2/import-book-exchange-rates?periodNumber=1234&baseCurrency={TestCurrencies.UKPound.Code}&exchangeCurrency={TestCurrencies.SwedishKrona.Code}",
                updateResource).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldUpdateExchangeRate()
        {
            this.DbContext.ImportBookExchangeRates
                .Should().ContainSingle(r =>
                    r.PeriodNumber == this.existingRate.PeriodNumber
                    && r.BaseCurrency == this.existingRate.BaseCurrency
                    && r.ExchangeCurrencyCode == this.existingRate.ExchangeCurrencyCode
                    && r.ExchangeRate == 13.75m);
        }

        [Test]
        public void ShouldReturnUpdatedResource()
        {
            var result = this.Response.DeserializeBody<ImportBookExchangeRateResource>();
            result.ExchangeRate.Should().Be(13.75m);
        }
    }
}
