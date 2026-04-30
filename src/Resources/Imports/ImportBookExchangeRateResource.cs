namespace Linn.Stores2.Resources.Imports
{
    using Linn.Common.Resources;

    public class ImportBookExchangeRateResource : HypermediaResource
    {
        public int PeriodNumber { get; set; }

        public string PeriodMonthName { get; set; }

        public string BaseCurrency { get; set; }

        public string ExchangeCurrencyCode { get; set; }

        public string ExchangeCurrencyName { get; set; }

        public decimal? ExchangeRate { get; set; }
    }
}
