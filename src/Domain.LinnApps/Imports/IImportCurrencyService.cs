namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public interface IImportCurrencyService
    {
        Task<LedgerPeriod> GetImportPeriod(DateTime customsDate);

        Task<ImportBookExchangeRate> GetExchangeRate(LedgerPeriod ledgerPeriod, Currency baseCurrency, Currency exchangeCurrency);
    }
}
