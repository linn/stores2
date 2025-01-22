namespace Linn.Stores2.Resources.Stores
{
    using System.Collections.Generic;

    using Linn.Common.Resources;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresBudgetResource : HypermediaResource
    {
        public int BudgetId { get; set; }

        public string TransactionCode { get; set; }

        public string TransactionCodeDescription { get; set; }

        public string PartNumber { get; set; }

        public PartResource Part { get; set; }

        public decimal Quantity { get; set; }

        public string Reference { get; set; }

        public decimal? PartPrice { get; set; }

        public int RequisitionNumber { get; set; }

        public int LineNumber { get; set; }

        public RequisitionHeaderResource Requisition { get; set; }

        public string CurrencyCode { get; set; }

        public decimal? MaterialPrice { get; set; }

        public decimal? LabourPrice { get; set; }

        public decimal? OverheadPrice { get; set; }

        public decimal? MachinePrice { get; set; }

        public decimal? SpotRate { get; set; }

        public string DateBooked { get; set; }

        public IEnumerable<StoresBudgetPostingResource> StoresBudgetPostings { get; set; }
    }
}
