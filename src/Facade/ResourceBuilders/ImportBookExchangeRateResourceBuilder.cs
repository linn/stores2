namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookExchangeRateResourceBuilder : IBuilder<ImportBookExchangeRate>
    {
        public ImportBookExchangeRateResource Build(ImportBookExchangeRate entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return null;
            }

            return new ImportBookExchangeRateResource
            {
                PeriodNumber = entity.PeriodNumber,
                PeriodMonthName = entity.LedgerPeriod?.MonthName,
                BaseCurrency = entity.BaseCurrency,
                ExchangeCurrencyCode = entity.ExchangeCurrencyCode,
                ExchangeCurrencyName = entity.ExchangeCurrency?.Name,
                ExchangeRate = entity.ExchangeRate,
                Links = new[]
                {
                    new LinkResource
                    {
                        Rel = "self",
                        Href = $"/stores2/import-book-exchange-rates/rate?periodNumber={entity.PeriodNumber}&baseCurrency={entity.BaseCurrency}&exchangeCurrency={entity.ExchangeCurrencyCode}"
                    }
                }
            };
        }

        object IBuilder<ImportBookExchangeRate>.Build(ImportBookExchangeRate entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        public string GetLocation(ImportBookExchangeRate entity) =>
            $"/stores2/import-book-exchange-rates/rate?periodNumber={entity.PeriodNumber}&baseCurrency={entity.BaseCurrency}&exchangeCurrency={entity.ExchangeCurrencyCode}";
    }
}
