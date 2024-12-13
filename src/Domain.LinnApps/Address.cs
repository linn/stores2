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

        public Address()
        {
        }

        public Address(
            string addressee,
            string line1,
            string line2,
            string line3,
            string line4,
            string postCode,
            Country country)
        {   
            this.Addressee = addressee;
            this.Line1 = line1;
            this.Line2 = line2;
            this.Line3 = line3;
            this.Line4 = line4;
            this.PostCode = postCode;
            this.Country = country;
        }
    }
}
