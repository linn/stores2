namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class StoresTransactionPosting
    {
        public StoresTransactionPosting()
        {
            // for ef
        }

        public StoresTransactionPosting(string transactionCode, string debitOrCredit, Nominal nominal)
        {
            this.TransactionCode = transactionCode;
            this.DebitOrCredit = debitOrCredit;
            this.Nominal = nominal;
        }

        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string DebitOrCredit { get; set; }

        public Nominal Nominal { get; set; }
    }
}
