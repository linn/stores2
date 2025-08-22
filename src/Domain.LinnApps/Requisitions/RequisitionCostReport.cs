namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;

    public class RequisitionCostReport
    {
        public IEnumerable<RequisitionCostLine> ReportLines { get; set; }

        public int ReqNumber { get; set; }

        public decimal TotalCost { get; set; }

        public decimal TotalNetCost { get; set; }
    }
}
