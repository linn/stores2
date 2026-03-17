namespace Linn.Stores2.TestData.SalesArticles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.TestData.Countries;
    using Linn.Stores2.TestData.Tariffs;

    public static class TestSalesArticles
    {
        public static readonly SalesArticle SelektHub =
            new SalesArticle
            {
                ArticleNumber = "SK HUB",
                Description = "SELEKT DSM HUB",
                Tariff = TestTariffs.SoundReproduction,
                TariffId = TestTariffs.SoundReproduction.TariffId,
                Weight = 9.4m,
                CountryOfOrigin = TestCountries.UnitedKingdom
            };

        public static readonly SalesArticle Akiva =
            new SalesArticle
            {
                ArticleNumber = "AKIVA",
                Description = "AKIVA Moving-coil cartridge",
                Tariff = TestTariffs.SoundReproduction,
                TariffId = TestTariffs.SoundReproduction.TariffId,
                Weight = 0.06m,
                CountryOfOrigin = TestCountries.Japan
            };

        public static readonly SalesArticle RadikalMachined =
            new SalesArticle
            {
                ArticleNumber = "RADIKAL/1/MH",
                Description = "RADIKAL - POWER SUPPLY FOR LP12 TURNTABLE MACHINED FROM SOLID",
                Tariff = TestTariffs.SoundReproduction,
                TariffId = TestTariffs.SoundReproduction.TariffId,
                Weight = 13.2m,
                CountryOfOrigin = TestCountries.UnitedKingdom
            };

        public static readonly SalesArticle RadikalUpg =
            new SalesArticle
            {
                ArticleNumber = "RADIKAL UPG",
                Description = "UPGRADE FOR RADIKAL",
                Tariff = TestTariffs.SoundReproduction,
                TariffId = TestTariffs.SoundReproduction.TariffId,
                Weight = 0.9m,
                CountryOfOrigin = TestCountries.UnitedKingdom
            };

        public static readonly SalesArticle K60Drum =
            new SalesArticle
            {
                ArticleNumber = "K60/DRUM",
                Description = "K60 TRI-WIRE SPEAKER CABLE (25M DRUM)",
                Tariff = TestTariffs.InsulatedWire,
                TariffId = TestTariffs.InsulatedWire.TariffId,
                CountryOfOrigin = TestCountries.China
            };

        public static readonly SalesArticle Spkr105 =
            new SalesArticle
            {
                ArticleNumber = "SPKR 105",
                Description = "Bass driver for the 119 loudspeaker",
                Tariff = TestTariffs.SingleSpeaker,
                TariffId = TestTariffs.SingleSpeaker.TariffId,
                CountryOfOrigin = TestCountries.Norway
            };
    }
}
