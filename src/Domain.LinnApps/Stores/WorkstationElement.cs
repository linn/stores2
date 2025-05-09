namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;

    public class WorkstationElement
    {
        public WorkstationElement()
        {
        }

        public WorkstationElement(
            int workStationElementId,
            string workStationCode,
            Employee createdBy,
            DateTime dateCreated,
            int? locationId,
            int? palletNumber)
        {
            this.WorkStationElementId = workStationElementId;
            this.WorkStationCode = workStationCode;
            this.CreatedBy = createdBy;
            this.DateCreated = dateCreated;
            this.LocationId = locationId;
            this.PalletNumber = palletNumber;
        }

        public int WorkStationElementId { get; protected set; }

        public string WorkStationCode { get; protected set; }

        public Employee CreatedBy { get; protected set; }

        public DateTime DateCreated { get; protected set; }

        public int? LocationId { get; protected set; }

        public int? PalletNumber { get; protected set; }
    }
}
