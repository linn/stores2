namespace Linn.Stores2.Integration.Tests.ImportBookExchangeRateModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Currencies;

    using NUnit.Framework;

    public class WhenGettingByPeriod : ContextBase
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
                "/stores2/import-book-exchange-rates/by-period?periodNumber=1234",
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
        public void ShouldReturnExchangeRate()
        {
            var results = this.Response.DeserializeBody<IEnumerable<ImportBookExchangeRateResource>>();
            results.Should().HaveCount(1);
            var result = results.First();
            result.PeriodNumber.Should().Be(this.exchangeRate.PeriodNumber);
            result.BaseCurrency.Should().Be(this.exchangeRate.BaseCurrency);
            result.ExchangeCurrencyCode.Should().Be(this.exchangeRate.ExchangeCurrencyCode);
            result.ExchangeRate.Should().Be(this.exchangeRate.ExchangeRate);
            result.ExchangeCurrencyName.Should().Be(TestCurrencies.SwedishKrona.Name);
        }
    }
}
