namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;

    public class WorkstationElement
    {
        public int WorkstationElementId { get; set; }

        public string WorkstationCode { get; set; }

        public Employee CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int? LocationId { get; set; }

        public int? PalletNumber { get; set; }
    }
}
