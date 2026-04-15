namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountingCompany { get; set; }

        public string ApprovedCarrier { get; set; }

        public Country Country { get; set; }

        public DateTime? DateClosed { get; set; }

        public Address OrderAddress { get; set; }

        public string NiceSupplierName => this.OrderAddress?.Addressee ?? this.Name;
    }
}
