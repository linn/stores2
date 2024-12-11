namespace Linn.Stores2.Domain.LinnApps
{
    public class Address
    {
        public int AddressId { get; protected set; }

        public string Addressee { get; protected set; }

        public string Line1 { get; protected set; }

        public string Line2 { get; protected set; }

        public string Line3 { get; protected set; }

        public string Line4 { get; protected set; }

        public string PostCode { get; protected set; }

        public Country Country { get; protected set; }
    }
}
