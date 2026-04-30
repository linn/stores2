namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenGettingByKey : ContextBase
    {
        private ImportBookExchangeRate exchangeRate;

        [SetUp]
        public void SetUp()
        {
            var period = new LedgerPeriod { PeriodNumber = 1234, MonthName = "January" };
            this.exchangeRate = new ImportBookExchangeRate
            {
                PeriodNumber = 1234,
                BaseCurrency = TestCurrencies.UKPound.Code,
                ExchangeCurrencyCode = TestCurrencies.SwedishKrona.Code,
                ExchangeCurrency = TestCurrencies.SwedishKrona,
                ExchangeRate = 12.5m
            };

            this.DbContext.LedgerPeriods.AddAndSave(this.DbContext, period);
            this.DbContext.Currencies.AddAndSave(this.DbContext, TestCurrencies.SwedishKrona);
            this.DbContext.ImportBookExchangeRates.AddAndSave(this.DbContext, this.exchangeRate);

            this.Response = this.Client.Get(
                $"/stores2/import-book-exchange-rates/rate?periodNumber=1234&baseCurrency={TestCurrencies.UKPound.Code}&exchangeCurrency={TestCurrencies.SwedishKrona.Code}",
                with => with.Accept("application/json")).Result;
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
        public void ShouldReturnCorrectExchangeRate()
        {
            var result = this.Response.DeserializeBody<ImportBookExchangeRateResource>();
            result.PeriodNumber.Should().Be(this.exchangeRate.PeriodNumber);
            result.BaseCurrency.Should().Be(this.exchangeRate.BaseCurrency);
            result.ExchangeCurrencyCode.Should().Be(this.exchangeRate.ExchangeCurrencyCode);
            result.ExchangeRate.Should().Be(this.exchangeRate.ExchangeRate);
            result.ExchangeCurrencyName.Should().Be(TestCurrencies.SwedishKrona.Name);
        }
    }
}
