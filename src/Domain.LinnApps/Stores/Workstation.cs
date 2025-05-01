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
            IEnumerable<WorkstationElement> elements)
        {
            this.WorkstationCode = workstationCode;
            this.Description = description;
            this.Cit = cit;
            this.VaxWorkstation = vaxWorkstation;
            this.ZoneType = zoneType;
            this.WorkstationElements = elements;
        }

        public string WorkstationCode { get; set; }

        public string Description { get; set; }

        public Cit Cit { get; set; }

        public string VaxWorkstation { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkstationElement> WorkstationElements { get; set; }

        public void Update(
            string workstationCode,
            string description,
            Cit cit,
            string vaxWorkstation,
            string zoneType,
            IEnumerable<WorkstationElement> elements)
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
