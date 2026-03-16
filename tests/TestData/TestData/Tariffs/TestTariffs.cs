namespace Linn.Stores2.TestData.Tariffs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps;

    public static class TestTariffs
    {
        public static readonly Tariff SoundReproduction =
            new Tariff
            {
                TariffId = 83,
                TariffCode = "8519 8900 00",
                ValidForIPR = "Y"
            };

        public static readonly Tariff InsulatedWire =
            new Tariff
            {
                TariffId = 240,
                TariffCode = "8544 4995 90"
            };

        public static readonly Tariff SingleSpeaker =
            new Tariff
            {
                TariffId = 228,
                TariffCode = "8518 2900 90",
                ValidForIPR = "Y"
            };
    }
}
