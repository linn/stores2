namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;

    public class ImportMaster
    {
        public Address Address { get; set; }

        public string TelephoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string VatRegistrationNumber { get; set; }

        // EORI = Economic Operators Registration and Identification number
        public string EORINumber { get; set; }

        public string DefermentAcct { get; set; }

        public string PVAText { get; set; }

        public string IPRDeclaration { get; set; }

        public string BRGDeclaration { get; set; }
    }
}
