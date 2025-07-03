namespace Linn.Stores2.Resources.Stores
{
    using Linn.Common.Resources;

    public class WorkStationElementResource : HypermediaResource
    {
        public int? WorkStationElementId { get; set; }

        public string WorkStationCode { get; set; }

        public int? CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public string DateCreated { get; set; }

        public int? LocationId { get; set; }

        public string LocationCode { get; set; }

        public string LocationDescription { get; set; }

        public int? PalletNumber { get; set; }
    }
}
