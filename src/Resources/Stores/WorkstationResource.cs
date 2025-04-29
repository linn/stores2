using System.Collections.Generic;

namespace Linn.Stores2.Resources.Stores
{
    public class WorkstationResource
    {
        public string WorkstationCode { get; set; }

        public string Description { get; set; }

        public string CitCode { get; set; }

        public string VaxWorkstation { get; set; }

        public string AlternativeWorkstationCode { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkstationElementResource> WorkstationElements { get; set; }
    }
}
