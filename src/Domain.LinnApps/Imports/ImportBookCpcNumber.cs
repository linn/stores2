namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;

    public class ImportBookCpcNumber
    {
        public int CpcNumber { get; set; }

        public string Description { get; set; }

        public DateTime? DateInvalid { get; set; }

        // new field to identify purpose of CPC either IPR/BRG/Material
        public string ReasonForImport { get; set; }

        public bool IsIPR => this.ReasonForImport == "IPR";
    }
}
