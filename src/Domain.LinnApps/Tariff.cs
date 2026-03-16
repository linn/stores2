namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class Tariff
    {
        public string TariffCode { get; set; }

        public int TariffId { get; set; }

        public string ValidForIPR { get; set; }

        public DateTime? DateInvalid { get; set; }

        public bool IsValidForIPR => this.ValidForIPR?.ToUpper() == "Y";
    }
}
