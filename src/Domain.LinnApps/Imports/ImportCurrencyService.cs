namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;

    public class ImportCurrencyService : IImportCurrencyService
    {
        private readonly IQueryRepository<LedgerPeriod> ledgerPeriodRepository;

        private readonly IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> importBookExchangeRateRepository;

        public ImportCurrencyService(
            IQueryRepository<LedgerPeriod> ledgerPeriodRepository,
            IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> importBookExchangeRateRepository)
        {
            this.ledgerPeriodRepository = ledgerPeriodRepository;
            this.importBookExchangeRateRepository = importBookExchangeRateRepository;
        }

        public async Task<LedgerPeriod> GetImportPeriod(DateTime customsDate)
        {
            var monthName = customsDate.ToString("MMMyyyy", CultureInfo.InvariantCulture);

            return await this.ledgerPeriodRepository.FindByAsync(p => p.MonthName == monthName);
        }

        public async Task<ImportBookExchangeRate> GetExchangeRate(
            LedgerPeriod ledgerPeriod,
            Currency baseCurrency,
            Currency exchangeCurrency)
        {
            if (baseCurrency == null || exchangeCurrency == null || ledgerPeriod == null)
            {
                return null;
            }

            // if the exchange currency is the same as the base currency, return an exchange rate of 1
            if (baseCurrency.Code == exchangeCurrency.Code)
            {
                return new ImportBookExchangeRate
                {
                    PeriodNumber = ledgerPeriod.PeriodNumber,
                    LedgerPeriod = ledgerPeriod,
                    BaseCurrency = baseCurrency.Code,
                    ExchangeCurrencyCode = exchangeCurrency.Code,
                    ExchangeRate = 1m
                };
            }

            var key = new ImportBookExchangeRateKey
            {
                PeriodNumber = ledgerPeriod.PeriodNumber,
                BaseCurrency = baseCurrency.Code,
                ExchangeCurrency = exchangeCurrency.Code
            };

            return await this.importBookExchangeRateRepository.FindByIdAsync(key);
        }
    }
}
