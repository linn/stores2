namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class StoresTransactionPosting
    {
        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string DebitOrCredit { get; set; }

        public Nominal Nominal { get; set; }
    }
}
