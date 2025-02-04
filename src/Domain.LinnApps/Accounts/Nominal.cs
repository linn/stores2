namespace Linn.Stores2.Domain.LinnApps.Accounts
{
    using System;

    public class Nominal
    {
        public Nominal()
        {
        }

        public Nominal(string code, string description)
        {
            this.NominalCode = code;
            this.Description = description;
            this.DateClosed = null;
        }

        public string NominalCode { get; set; }

        public string Description { get; set; }

        public DateTime? DateClosed { get; set; }
    }
}
