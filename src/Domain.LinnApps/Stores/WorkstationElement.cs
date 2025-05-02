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

        public int WorkstationElementId { get; protected set; }

        public string WorkstationCode { get; protected set; }

        public Employee CreatedBy { get; protected set; }

        public DateTime DateCreated { get; protected set; }

        public int? LocationId { get; protected set; }

        public int? PalletNumber { get; protected set; }
    }
}
