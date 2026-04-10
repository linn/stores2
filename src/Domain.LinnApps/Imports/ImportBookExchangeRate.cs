namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;

    public class ImportBookExchangeRate
    {
        public int PeriodNumber { get; set; }

        public LedgerPeriod LedgerPeriod { get; set; }

        public string ExchangeCurrencyCode { get; set; }

        public Currency ExchangeCurrency { get; set; }

        public string BaseCurrency { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? ConvertToBaseValue(decimal? value)
        {
            if (this.ExchangeRate == null || value == null)
            {
                return value;
            }

            return Math.Round(value.Value / this.ExchangeRate.Value, 2);
        }
    }
}
