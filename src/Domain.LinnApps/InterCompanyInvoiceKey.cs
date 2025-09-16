namespace Linn.Stores2.Domain.LinnApps
{
    public class InterCompanyInvoiceKey
    {
        public InterCompanyInvoiceKey(string documentType, int documentNumber)
        {
            this.DocumentType = documentType;
            this.DocumentNumber = documentNumber;
        }

        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }
    }
}
