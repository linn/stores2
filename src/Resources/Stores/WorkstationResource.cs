namespace Linn.Stores2.Resources.Stores
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class WorkStationResource : HypermediaResource
    {
        public string WorkStationCode { get; set; }

        public string Description { get; set; }

        public string CitCode { get; set; }

        public string CitName { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkStationElementResource> WorkStationElements { get; set; }
    }
}
