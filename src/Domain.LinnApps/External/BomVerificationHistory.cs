namespace Linn.Stores2.Domain.LinnApps.External
{
    using System;

    public class BomVerificationHistory
    {
        public int TRef { get; set; }

        public string PartNumber { get; set; }

        public int VerifiedBy { get; set; }

        public DateTime DateVerified { get; set; }

        public string DocumentType { get; set; }

        public int? DocumentNumber { get; set; }

        public string Remarks { get; set; }
    }
}
