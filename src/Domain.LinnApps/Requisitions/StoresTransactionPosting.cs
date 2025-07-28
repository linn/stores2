namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class StoresTransactionPosting
    {
        public StoresTransactionPosting(string transactionCode, string debitOrCredit, Nominal nominal)
        {
            this.TransactionCode = transactionCode;
            this.DebitOrCredit = debitOrCredit;
            this.Nominal = nominal;
        }

        protected StoresTransactionPosting()
        {
            // for ef
        }

        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string DebitOrCredit { get; set; }

        public Nominal Nominal { get; set; }

        public string NominalRule { get; set; }

        public string DepartmentRule { get; set; }
    }
}
