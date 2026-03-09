namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;

    public class ImportCandidate
    {
        public int Id { get; set; }

        public Employee CreatedBy { get; set; }

        public Supplier Supplier { get; set; }

        public Supplier Carrier { get; set; }

        public Currency Currency { get; set; }

        public Currency BaseCurrency { get; set; }

        public Currency ExchangeCurrency { get; set; }

        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public decimal? LinnDuty { get; set; }

        public decimal? LinnVat { get; set; }
    }
}
