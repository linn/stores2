namespace Linn.Stores2.Resources.Requisitions
{
    using Linn.Stores2.Resources.Accounts;

    public class RequisitionLinePostingResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public int Sequence { get; set; }

        public decimal? Quantity { get; set; }

        public string DebitOrCredit { get; set; }

        public string NominalCode { get; set; }

        public string DepartmentCode { get; set; }

        public NominalAccountResource NominalAccount { get; set; }
    }
}
