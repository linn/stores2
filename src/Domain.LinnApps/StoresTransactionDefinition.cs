namespace Linn.Stores2.Domain.LinnApps
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
    }
}
