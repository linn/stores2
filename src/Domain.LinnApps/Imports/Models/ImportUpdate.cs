namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;

    public class ImportUpdate
    {
        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public LedgerPeriod CustomsPeriod { get; set; }

        public Currency Currency { get; set; }

        public ImportBookExchangeRate ExchangeRate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public string TransportBillNumber { get; set; }

        public string Comments { get; set; }

        public string ClearanceComments { get; set; }

        public Employee CancelledBy { get; set; }

        public string CancelledReason { get; set; }

        public DateTime? DateReceived { get; set; }

        public DateTime? DateInstructionSent { get; set; }

        public IList<ImportOrderDetailCandidate> OrderDetailCandidates { get; set; }
    }
}
