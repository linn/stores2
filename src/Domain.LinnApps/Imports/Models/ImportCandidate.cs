namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    public class ImportCandidate
    {
        public int Id { get; set; }

        public Employee CreatedBy { get; set; }

        public Supplier Supplier { get; set; }

        public Supplier Carrier { get; set; }
    }
}
