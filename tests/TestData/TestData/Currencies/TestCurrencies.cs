namespace Linn.Stores2.TestData.Currencies
{
    using Linn.Stores2.Domain.LinnApps;

    public static class TestCurrencies
    {
        public static readonly Currency UKPound = new Currency { Code = "GBP", Name = "British Pound" };

        public static readonly Currency USDollar = new Currency { Code = "USD", Name = "American Dollars" };

        public static readonly Currency SwedishKrona = new Currency { Code = "SEK", Name = "Swedish Krona" };
    }
}
