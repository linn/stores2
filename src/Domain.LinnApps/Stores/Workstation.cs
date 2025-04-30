namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Workstation
    {
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

            if (elements != null)
            {
                this.SetElements(elements);
            }
        }

        public string WorkstationCode { get; set; }

        public string Description { get; set; }

        public Cit Cit { get; set; }

        public string VaxWorkstation { get; set; }

        public string ZoneType { get; set; }

        public IEnumerable<WorkstationElement> WorkstationElements { get; set; }

        private void SetElements(IEnumerable<WorkstationElement> elements)
        {
            var workstationElements = elements.ToList();
            foreach (var e in workstationElements)
            {

            }
        }
    }
}
