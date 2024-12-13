namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class Carrier
    {
        public string CarrierCode { get; protected init; }

        public string Name { get; set; }

        public DateTime DateCreated { get; protected set; }

        public DateTime? DateInvalid { get; protected set; }

        public Organisation Organisation { get; protected set; }
        
        public Carrier()
        {
        }
        
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
            this.CarrierCode = code;
            this.Name = name;

            var address = new Address(
                addressee,
                addressLine1,
                addressLine2,
                addressLine3,
                addressLine4,
                postCode,
                country);
            
            this.Organisation = new Organisation(name, vrn, phoneNumber, address);
            this.DateCreated = DateTime.Now;
            this.Validate();
        }

        public void Update(string name)
        {
            if (name.ToUpper().Trim() != this.Name)
            {
                this.Name = name.ToUpper().Trim();
                this.Validate();
            }
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new CarrierException("Name is required");
            }
        }
    }
}
