namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Collections.Generic;

    public class Workstation
    {
        public string WorkstationCode { get; set; }

        public string Description { get; set; }

        public Cit Cit { get; set; }

        public string VaxWorkstation { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkstationElement> WorkstationElements { get; set; }
    }
}
