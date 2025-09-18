namespace Linn.Stores2.Domain.LinnApps.Parts
{
    public class Bom
    {
        public int BomId { get; set; }

        public string BomName { get; set; }

        public decimal LabourTimeMins { get; set; }

        public decimal TotalLabourTimeMins { get; set; }

        public Part Part { get; set; }

        public decimal TotalLabourTimeHours => this.TotalLabourTimeMins / 60;
    }
}
