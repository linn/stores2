using System.Collections.Generic;

namespace Linn.Stores2.Resources.External
{
    public class PurchaseOrderResource
    {
        public int OrderNumber { get; set; }

        public string DateFilCancelled { get; set; }

        public EmployeeResource AuthorisedBy { get; set; }
        
        public PurchaseOrderTypeResource DocumentType { get; set; }

        public IEnumerable<PurchaseOrderDetailResource> Details { get; set; }
    }
}
