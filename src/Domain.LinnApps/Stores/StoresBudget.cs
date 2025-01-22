namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System;
    using System.Collections.ObjectModel;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class StoresBudget
    {
        public int BudgetId { get; set; }

        public string TransactionCode { get; set; }

        public string PartNumber { get; set; }

        public Part Part { get; set; }

        public decimal Quantity { get; set; }

        public string Reference { get; set; }

        public decimal? PartPrice { get; set; }

        public int RequisitionNumber { get; set; }

        public int LineNumber { get; set; }

        public RequisitionHeader Requisition { get; set; }

        public string CurrencyCode { get; set; }

        public decimal? MaterialPrice { get; set; }

        public decimal? LabourPrice { get; set; }

        public decimal? OverheadPrice { get; set; }

        public decimal? MachinePrice { get; set; }

        public decimal? SpotRate { get; set; }

        public DateTime? DateBooked { get; set; }

        public Collection<StoresBudgetPosting> StoresBudgetPostings { get; set; }
    }
}
