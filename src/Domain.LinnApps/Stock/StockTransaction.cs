namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System;

    public class StockTransaction
    {
        public string TransactionCode { get; set;  }

        public int BudgetId { get; set; }

        public int ReqNumber { get; set; }

        public string Document1 { get; set; }

        public string Document1Line { get; set; }

        public string FunctionCode { get; set; }

        public string PartNumber { get; set; }

        public decimal Quantity { get; set; }

        public int? Amount { get; set; }

        public DateTime? BudgetDate { get; set; }

        public int BookedById { get; set; }

        public Employee BookedBy { get; set; }

        public string ReqReference { get; set; }

        public string DebitOrCredit { get; set; }
    }
}
