﻿namespace Linn.Stores2.Domain.LinnApps.Stores
{
    public class StoresTransactionState
    {
        public StoresTransactionState(string fromOrOnto, string transactionCode, string state)
        {
            this.FromOrOnto = fromOrOnto;
            this.TransactionCode = transactionCode;
            this.State = state;
        }

        // for ef
        protected StoresTransactionState()
        {
        }

        public string FromOrOnto { get; set; }

        public string TransactionCode { get; set; }
     
        public string State { get; set; }
    }
}
