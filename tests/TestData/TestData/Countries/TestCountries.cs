namespace Linn.Stores2.TestData.Countries
{
    using Linn.Stores2.Domain.LinnApps;

    public class TestCountries
    {
        public static Country UnitedKingdom => new Country("GB", "UNITED KINGDOM", "United Kingdom", "N");

        public static Country Sweden => new Country("SE", "SWEDEN", "Sweden", "Y");

        public static Country Ireland => new Country("IE", "IRELAND", "Ireland", "Y");

        public static Country UnitedStates => new Country("US", "UNITED STATES OF AMERICA", "United States Of America", "N");
    }
}
