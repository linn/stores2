namespace Linn.Stores2.Domain.LinnApps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SalesOutlet
    {
        public int AccountId { get; set; }

        public int OutletNumber { get; set; }

        public string Name { get; set; }

        public Address OutletAddress { get; set; }

        public DateTime? DateInvalid { get; set; }

        public string CarrierCode { get; set; }

        public string Terms { get; set; }

        public bool ExportOutlet => this.OutletAddress != null && this.OutletAddress.Country != null && !this.OutletAddress.Country.IsGreatBritain;

        public bool EuOutlet => this.OutletAddress != null && this.OutletAddress.Country != null && this.OutletAddress.Country.IsEuMember;
    }
}
