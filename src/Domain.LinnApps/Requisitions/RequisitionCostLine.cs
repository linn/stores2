namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class RequisitionCostLine
    {
        public int? ReqNumber { get; set; }

        public string PartNumber { get; set; }

        public string Description { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Cost { get; set; }

        public decimal? NetCost { get; set; }
    }
}
