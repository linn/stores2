namespace Linn.Stores2.Resources.External
{
    public class WorksOrderResource
    {
        public int OrderNumber { get; set; }

        public string DateCancelled { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public string WorkStationCode { get; set; }

        public string Outstanding { get; set; }

        public decimal Quantity { get; set; }

        public decimal QuantityBuilt { get; set; }
    }
}
