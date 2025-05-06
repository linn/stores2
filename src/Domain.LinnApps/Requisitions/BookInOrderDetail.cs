namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class BookInOrderDetail
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }
        
        public int Sequence { get; set; }

        public decimal? Quantity { get; set; }

        public string DepartmentCode { get; set; }
        
        public string NominalCode { get; set; }

        public string PartNumber { get; set; }

        public int? ReqNumber { get; set; }

        public string IsReverse { get; set; }
    }
}
