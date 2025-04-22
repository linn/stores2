using System.Collections.Generic;
using System.Linq;

namespace Linn.Stores2.Domain.LinnApps.External
{
    public class PurchaseOrderResult
    {
        public int OrderNumber { get; set; }

        public bool IsFilCancelled { get; set; }

        public bool IsAuthorised { get; set; }
        
        public string DocumentType { get; set; }

        public IEnumerable<PurchaseOrderDetailResult> Details { get; set; }

        public decimal? OrderQty(int? lineNumber = null)
        {
            var detail = this.Details.SingleOrDefault(d => d.Line == (lineNumber ?? 1));
            return detail?.OurQty;
        }
    }
}
