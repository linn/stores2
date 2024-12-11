namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class Carrier
    {
        public string CarrierCode { get; protected init; }

        public string Name { get; set; }

        public DateTime DateCreated { get; protected set; }

        public DateTime DateInvalid { get; protected set; }

        public Organisation Organisation { get; protected set; }

        public Carrier(
            string code,
            string name,
            string addressee,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string postCode,
            Country country,
            string phoneNumber,
            string vrn)
        {
            // todo
        }
    }
}
