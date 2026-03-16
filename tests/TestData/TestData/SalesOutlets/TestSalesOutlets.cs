namespace Linn.Stores2.TestData.SalesOutlets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.TestData.Countries;

    public static class TestSalesOutlets
    {
        public static readonly SalesOutlet PeterTyson =
            new SalesOutlet
            {
                AccountId = 18706,
                OutletNumber = 1,
                Name = "Peter Tyson (Carlisle)",
                CarrierCode = "TNT",
                OutletAddress = new Address("Peter Tyson", "Unit 2", "Main Street", "Mull", null, "PA75 6NY", TestCountries.UnitedKingdom)
            };

        public static readonly SalesOutlet PeterTysonClosed =
            new SalesOutlet
            {
                AccountId = 18706,
                OutletNumber = 2,
                Name = "Peter Tyson (Balamory)",
                DateInvalid = DateTime.Today,
                CarrierCode = "TNT",
                OutletAddress = new Address("Peter Tyson", "Unit 2", "Main Street", "Mull", null, "PA75 6NY", TestCountries.UnitedKingdom)
            };

        public static readonly SalesOutlet TonlagetHifi =
            new SalesOutlet
            {
                AccountId = 18649,
                OutletNumber = 2,
                Name = "Tonlaget Hi-Fi AB",
                CarrierCode = "TNT",
                Terms = "D.D.P.",
                OutletAddress = new Address("Tonlaget Hi-Fi Ab", "Aschebergsgatan 15", "Goteborg", null, null, "411 27", TestCountries.Sweden)
            };

        public static readonly SalesOutlet AccentOnMusic =
            new SalesOutlet
            {
                AccountId = 65140,
                OutletNumber = 1,
                Name = "Accent On Music",
                CarrierCode = "FEDEX",
                Terms = "D.D.P.",
                OutletAddress = new Address("Accent on Music", "175 Main Street", "Mount Kisco", null, null, "NY 10549", TestCountries.UnitedStates)
            };

    }
}
