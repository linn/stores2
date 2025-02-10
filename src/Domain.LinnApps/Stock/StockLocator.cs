namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Parts;

    public class StockLocator
    {
        public int Id { get; set; }

        public decimal? Quantity { get; set; }

        public int? PalletNumber { get; set; }

        public int? LocationId { get; set; }

        public StorageLocation StorageLocation { get; set; }

        public int? BudgetId { get; set; }

        public string PartNumber { get; set; }

        public Part Part { get; set; }

        public decimal? QuantityAllocated { get; set; }

        public string StockPoolCode { get; set; }

        public string Remarks { get; set; }

        public DateTime? StockRotationDate { get; set; }

        public string BatchRef { get; set; }

        public string State { get; set; }

        public string Category { get; set; }

        public string CurrentStock { get; set; }
    }
}
