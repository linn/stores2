namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;

    public class ImportUpdate
    {
        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public LedgerPeriod Period { get; set; }

        public Currency Currency { get; set; }

        public ImportBookExchangeRate ExchangeRate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public decimal? LinnDuty { get; set; }

        public decimal? LinnVat { get; set; }

        public string TransportBillNumber { get; set; }

        public DateTime? DateReceived { get; set; }

        public IList<ImportOrderDetailCandidate> OrderDetailCandidates { get; set; }
    }
}
