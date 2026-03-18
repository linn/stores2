namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Returns;

    public class ImportInvoiceDetailCandidate
    {
        public ImportInvoiceDetailCandidate(string invoiceNumber, decimal invoiceValue)
        {
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceValue = invoiceValue;
        }

        public ImportInvoiceDetailCandidate(Rsn rsn)
        {
            this.InvoiceNumber = rsn.RsnNumber.ToString();
            this.Currency = rsn.CustomsCurrency();
            this.InvoiceValue = rsn.CustomsValue() ?? 0;
        }

        public int ImportBookId { get; set; }

        public string InvoiceNumber { get; set; }

        public Currency Currency { get; set; }

        public decimal InvoiceValue { get; set; }
    }
}
