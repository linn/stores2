namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImportAuthNumber
    {
        public int Id { get; set; }

        // mainly IPR and BRG authorisation numbers, but could be used for other types in future if needed
        public string AuthorisationType { get; set; }

        public string AuthorisationNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool Matches(DateTime matchDate)
        {
            return matchDate >= this.FromDate && (this.ToDate == null || matchDate <= this.ToDate);
        }

    }
}
