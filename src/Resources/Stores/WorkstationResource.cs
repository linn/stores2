namespace Linn.Stores2.Resources.Stores
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class WorkstationResource : HypermediaResource
    {
        public string WorkstationCode { get; set; }

        public string Description { get; set; }

        public string CitCode { get; set; }

        public string CitName { get; set; }

        public string VaxWorkstation { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkstationElementResource> WorkstationElements { get; set; }
    }
}
