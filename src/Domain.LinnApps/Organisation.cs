namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class Organisation
    {
        public int OrgId { get; protected set; }

        public string Title { get; protected set; }

        public string VatRegistrationNumber { get; protected set; }

        public string PhoneNumber { get; protected set; }

        public Address Address { get; protected set; } 
        
        public DateTime Created { get; protected set; }
    }
}
