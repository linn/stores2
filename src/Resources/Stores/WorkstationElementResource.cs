﻿namespace Linn.Stores2.Resources.Stores
{
    using Linn.Common.Resources;

    public class WorkstationElementResource : HypermediaResource
    {
        public int WorkstationElementId { get; set; }

        public string WorkstationCode { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string DateCreated { get; set; }

        public int? LocationId { get; set; }

        public int? PalletNumber { get; set; }
    }
}
