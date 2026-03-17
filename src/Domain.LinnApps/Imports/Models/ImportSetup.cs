namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ImportSetup
    {
        public ImportSetup()
        {
            this.Rsns = new List<Rsn>();
        }

        public Employee CreatedBy { get; set; }

        public IList<Rsn> Rsns { get; private set; }

        public void AddRsn(Rsn rsn)
        {
            this.Rsns.Add(rsn);
        }

        public IList<ImportOrderDetailCandidate> OrderDetailCandidates()
        {
            if (this.Rsns.Any())
            {
                return this.Rsns.Select(rsn => new ImportOrderDetailCandidate(rsn)).ToList();
            }

            return new List<ImportOrderDetailCandidate>();
        }
    }
}
