using Linn.Stores2.Domain.LinnApps.Accounts;

namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class StoresTransactionPosting
    {
        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string DebitOrCredit { get; set; }

        public Nominal Nominal { get; set; }
    }
}
