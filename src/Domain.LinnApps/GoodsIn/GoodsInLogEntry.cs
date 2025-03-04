namespace Linn.Stores2.Domain.LinnApps.GoodsIn
{
    using System;

    public class GoodsInLogEntry
    {
        public int Id { get; set;  }

        public string TransactionType { get; set; }

        public DateTime DateCreated { get; set; }

        public Employee CreatedBy { get; set; }

        public string WandString { get; set; }

        public string ArticleNumber { get; set; }

        public decimal? Quantity { get; set; }
        
        public int? SerialNumber { get; set; }

        public int? OrderNumber { get; set; }

        public int? OrderLine { get; set; }

        public int? LoanNumber { get; set; }

        public int? LoanLine { get; set; }

        public int? RsnNumber { get; set; }

        public int? ReqNumber { get; set; }

        public int? ReqLine { get; set; }

        public int? SernosTref { get; set; }

        public string ProductAnalysisCode { get; set; }

        public string StoragePlace { get; set; }

        public string Processed { get; set; }

        public string ErrorMessage { get; set; }

        public int? BookInRef { get; set; }

        public string DemLocation { get; set; }

        public string Comments { get; set; }

        public string State { get; set; }

        public string StorageType { get; set; }

        public string ManufacturersPartNumber { get; set; }

        public string LogCondition { get; set; }

        public string RsnAccessories { get; set; }
    }
}
