namespace Linn.Stores2.Resources.Requisitions
{
    public class RequisitionLinePostingResource
    {
        public int ReqNumber { get; set; }

        public int LineNumber { get; set; }

        public int Seq { get; set; }

        public decimal? Qty { get; set; }

        public string DebitOrCredit { get; set; }

        public string NominalCode{ get; set; }

        public string DepartmentCode { get; set; }
    }
}
