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
                    Country = TestCountries.UnitedKingdom,
                    ApprovedCarrier = "Y"
            };

        public static readonly Supplier DHLLogistics =
            new Supplier
                {
                    Id = 1054,
                    Name = "DHL LOGISTICS (UK) LTD",
                    AccountingCompany = "LINN",
                    Country = TestCountries.UnitedKingdom,
                    ApprovedCarrier = "Y"
                };

        // Retailers for RSN type Imports
        public static readonly Country TaktAndTonCountry = TestCountries.Sweden;

        public static readonly Supplier TaktAndTon =
            new Supplier
                {
                    Id = 54290,
                    Name = "TAKT AND TON (SEK ACCOUNT)",
                    AccountingCompany = "LINN",
                    Country = TaktAndTonCountry,
                    ApprovedCarrier = "N",
                    OrderAddress = new Address("Takt and Ton", "Box 1102", "S-164 22 Kista", string.Empty, string.Empty, string.Empty, TaktAndTonCountry)
                };

        public static readonly Supplier LinnJapan =
            new Supplier
            {
                Id = 104164,
                Name = "LINN JAPAN",
                AccountingCompany = "LINN",
                Country = TestCountries.Japan,
                ApprovedCarrier = "N"
            };

        // Suppliers for PO imports
        public static readonly Supplier SeasFabrikker =
            new Supplier
            {
                Id = 121628,
                Name = "SEAS FABRIKKER AS (EURO ACCOUNT)",
                AccountingCompany = "LINN",
                Country = TestCountries.Norway,
                ApprovedCarrier = "N"
            };

        // Suppliers for SUNDRY, SAMPLES type imports
        public static readonly Supplier AcmeIncorporated =
            new Supplier
            {
                Id = 146737,
                Name = "ACME INCORPORATED (IMPORT ONLY)",
                AccountingCompany = "LINN",
                Country = TestCountries.China,
                ApprovedCarrier = "N"
            };
    }
}
