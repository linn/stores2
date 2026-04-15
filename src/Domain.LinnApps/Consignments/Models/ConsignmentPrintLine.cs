namespace Linn.Stores2.Domain.LinnApps.Consignments.Models
{
    public class ConsignmentPrintLine
    {
        public string ItemDescription { get; set; }

        public int? LowValue { get; set; }

        public int? HighValue { get; set; }

        public int Count { get; set; }

        public decimal? Weight { get; set; }

        public string Dims { get; set; }

        public decimal? Volume { get; set; }
    }
}
