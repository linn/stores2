namespace Linn.Stores2.Domain.LinnApps.Logistics
{
    public class ImportBookExchangeRate
    {
        public int PeriodNumber { get; set; }

        public LedgerPeriod LedgerPeriod { get; set; }

        public string ExchangeCurrency { get; set; }

        public string BaseCurrency { get; set; }

        public decimal? ExchangeRate { get; set; }
    }
}
