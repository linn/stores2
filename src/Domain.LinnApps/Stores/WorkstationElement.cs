namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

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
            StorageLocation storageLocation,
            StoresPallet storesPallet)
        {
            if (storageLocation == null && storesPallet == null)
            {
                throw new WorkstationException("A workstation element must have either a storage location, a pallet, or both.");
            }

            this.WorkStationElementId = workStationElementId;
            this.WorkStationCode = workStationCode;
            this.CreatedBy = createdBy;
            this.DateCreated = dateCreated;
            this.StorageLocation = storageLocation;
            this.Pallet = storesPallet;
        }

        public int WorkStationElementId { get; protected set; }

        public string WorkStationCode { get; protected set; }

        public Employee CreatedBy { get; protected set; }

        public DateTime DateCreated { get; protected set; }

        public StorageLocation StorageLocation { get; protected set; }

        public StoresPallet Pallet { get; protected set; }
    }
}
