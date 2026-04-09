namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;

    public class ImportUpdate
    {
        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public decimal? LinnDuty { get; set; }

        public decimal? LinnVat { get; set; }

        public string TransportBillNumber { get; set; }

        public DateTime? DateReceived { get; set; }
    }
}
