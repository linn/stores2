namespace Linn.Stores2.Domain.LinnApps.Stores
{
    public class StoresTransactionState
    {
        protected StoresTransactionState()
        {
            // for ef
        }

        public StoresTransactionState(string fromOrOnto, string transactionCode, string state)
        {
            this.FromOrOnto = fromOrOnto;
            this.TransactionCode = transactionCode;
            this.State = state;
        }

        public string FromOrOnto { get; set; }

        public string TransactionCode { get; set; }
     
        public string State { get; set; }
    }
}
