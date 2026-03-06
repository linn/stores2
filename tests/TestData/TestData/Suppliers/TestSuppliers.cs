namespace Linn.Stores2.TestData.Suppliers
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.TestData.Countries;

    public static class TestSuppliers
    {
        // Carriers
        public static readonly Supplier Fedex =
            new Supplier
            {
                    Id = 29407,
                    Name = "FEDERAL EXPRESS UK LTD",
                    AccountingCompany = "LINN",
                    CountryCode = TestCountries.UnitedKingdom.CountryCode,
                    Country = TestCountries.UnitedKingdom,
                    ApprovedCarrier = "Y"
            };

        public static readonly Supplier DHLLogistics =
            new Supplier
                {
                    Id = 1054,
                    Name = "DHL LOGISTICS (UK) LTD",
                    AccountingCompany = "LINN",
                    CountryCode = TestCountries.UnitedKingdom.CountryCode,
                    Country = TestCountries.UnitedKingdom,
                    ApprovedCarrier = "Y"
                };

        // Retailers for RSN type Imports
        public static readonly Supplier TaktAndTon =
            new Supplier
                {
                    Id = 54290,
                    Name = "TAKT AND TON",
                    AccountingCompany = "LINN",
                    CountryCode = TestCountries.Sweden.CountryCode,
                    Country = TestCountries.Sweden,
                    ApprovedCarrier = "N"
                };
    }
}
