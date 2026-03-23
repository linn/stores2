namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;

    public class ImportBookCpcNumber
    {
        public int CpcNumber { get; set; }

        public string Description { get; set; }

        public DateTime? DateInvalid { get; set; }

        public bool IsIPR => this.Description == "51 00 00";
    }
}
