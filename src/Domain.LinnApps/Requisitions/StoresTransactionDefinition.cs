namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class StoresTransactionDefinition
    {
        public StoresTransactionDefinition()
        {
        }

        public StoresTransactionDefinition(string transactionCode)
        {
            this.TransactionCode = transactionCode;
        }

        public string TransactionCode { get; set; }

        public string Description { get; set; }

        public string QcType { get; set; }

        public string DocType { get; set; }

        public string StockAllocations { get; set; }

        public string OntoTransactions { get; set; }

        public string DecrementTransaction { get; set; }

        public string TakePriceFrom { get; set; }

        public bool RequiresStockAllocations => this.StockAllocations == "Y";

        public bool RequiresOntoTransactions => this.OntoTransactions == "Y";

        public bool IsDecrementTransaction => this.DecrementTransaction == "Y";

        public bool RequiresMoves => this.RequiresStockAllocations || this.RequiresOntoTransactions;

        public bool MaterialVarianceTransaction => this.TakePriceFrom == "M";
    }
}
