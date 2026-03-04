using Linn.Common.Resources;

namespace Linn.Stores2.Resources.Imports
{
    public class ImportBookInvoiceDetailResource : HypermediaResource
    {
        public int ImportBookId { get; set; }

        public int LineNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal InvoiceValue { get; set; }
    }
}
