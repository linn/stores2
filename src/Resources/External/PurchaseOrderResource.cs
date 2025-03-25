namespace Linn.Stores2.Resources.External
{
    public class PurchaseOrderResource
    {
        public int OrderNumber { get; set; }

        public string DateFilCancelled { get; set; }

        public EmployeeResource AuthorisedBy { get; set; }
    }
}
