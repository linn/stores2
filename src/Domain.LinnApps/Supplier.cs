namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountingCompany { get; set; }

        public string ApprovedCarrier { get; set; }

        public string CountryCode { get; set; }

        public DateTime? DateClosed { get; set; }
    }
}
