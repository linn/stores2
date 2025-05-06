namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Collections.Generic;

    public class Workstation
    {
        public Workstation()
        {
        }

        public Workstation(
            string workstationCode,
            string description,
            Cit cit,
            string vaxWorkstation,
            string zoneType,
            ICollection<WorkstationElement> elements)
        {
            this.WorkstationCode = workstationCode;
            this.Description = description;
            this.Cit = cit;
            this.VaxWorkstation = vaxWorkstation;
            this.ZoneType = zoneType;
            this.WorkstationElements = elements;
        }

        public string WorkstationCode { get; protected set; }

        public string Description { get; protected set; }

        public Cit Cit { get; protected set; }

        public string VaxWorkstation { get; protected set; }

        public string ZoneType { get; protected set; }

        public ICollection<WorkstationElement> WorkstationElements { get; protected set; }

        public void Update(
            string workstationCode,
            string description,
            Cit cit,
            string vaxWorkstation,
            string zoneType,
            ICollection<WorkstationElement> elements)
        {
            this.WorkstationCode = workstationCode;
            this.Description = description;
            this.Cit = cit;
            this.VaxWorkstation = vaxWorkstation;
            this.ZoneType = zoneType;
            this.WorkstationElements = elements;
        }
    }
}
