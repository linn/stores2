namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class RequisitionLinePosting
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public int Seq { get; set; }

        public decimal? Qty { get; set; }

        public string DebitOrCredit { get; set; }

        public NominalAccount NominalAccount { get; set; }
    }
}
