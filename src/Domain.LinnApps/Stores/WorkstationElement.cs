namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;

    public class WorkstationElement
    {
        public WorkstationElement()
        {
        }

        public WorkstationElement(
            int workstationElementId,
            string workstationCode,
            Employee createdBy,
            DateTime dateCreated,
            int? locationId,
            int? palletNumber)
        {
            this.WorkstationElementId = workstationElementId;
            this.WorkstationCode = workstationCode;
            this.CreatedBy = createdBy;
            this.DateCreated = dateCreated;
            this.LocationId = locationId;
            this.PalletNumber = palletNumber;
        }

        public int WorkstationElementId { get; set; }

        public string WorkstationCode { get; set; }

        public Employee CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int? LocationId { get; set; }

        public int? PalletNumber { get; set; }
    }
}
