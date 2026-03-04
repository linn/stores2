namespace Linn.Stores2.TestData.Suppliers
{
    using Linn.Stores2.Domain.LinnApps;

    public static class TestSuppliers
    {
        // Carriers

        public static readonly Supplier Fedex =
            new Supplier
            {
                    Id = 29407,
                    Name = "FEDERAL EXPRESS UK LTD",
                    AccountingCompany = "LINN",
                    CountryCode = "GB",
                    ApprovedCarrier = "Y"
            };

        public static readonly Supplier DHLLogistics =
            new Supplier
                {
                    Id = 1054,
                    Name = "DHL LOGISTICS (UK) LTD",
                    AccountingCompany = "LINN",
                    CountryCode = "GB",
                    ApprovedCarrier = "Y"
                };

        // Retailers for RSN type Imports

        public static readonly Supplier TaktAndTon =
            new Supplier
                {
                    Id = 54290,
                    Name = "TAKT AND TON",
                    AccountingCompany = "LINN",
                    CountryCode = "SE",
                    ApprovedCarrier = "N"
                };
    }
}
