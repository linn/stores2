namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Threading.Tasks;

    public interface IImportCurrencyService
    {
        Task<LedgerPeriod> GetImportPeriod(DateTime customsDate);

        Task<ImportBookExchangeRate> GetExchangeRate(
            LedgerPeriod ledgerPeriod,
            Currency baseCurrency,
            Currency exchangeCurrency);
    }
}
