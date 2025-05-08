namespace Linn.Stores2.Resources.Requisitions
{
    public class SundryBookInDetailResource
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }
        
        public decimal? Quantity { get; set; }

        public int? ReqNumber { get; set; }
        
        public int? LineNumber { get; set; }

        public string TransactionReference { get; set; }

        public string DepartmentCode { get; set; }
        
        public string NominalCode { get; set; }
    }
}
