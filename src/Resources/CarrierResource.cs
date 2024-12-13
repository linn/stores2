namespace Linn.Stores2.Resources
{
    using Linn.Common.Resources;

    public class CarrierResource : HypermediaResource
    {
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string VatRegistrationNumber { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Addressee { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string PostCode { get; set; }
        
        public string CountryCode { get; set; }
    }
}
