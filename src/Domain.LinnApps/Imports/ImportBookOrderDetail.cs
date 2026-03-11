namespace Linn.Stores2.Domain.LinnApps.Imports
{
    public class ImportBookOrderDetail
    {
        public int? CpcNumber { get; set; }

        public decimal DutyValue { get; set; }

        public decimal FreightValue { get; set; }

        public int ImportBookId { get; set; }

        public int? InsNumber { get; set; }

        public int LineNumber { get; set; }

        public string LineType { get; set; }

        public int? LoanNumber { get; set; }

        public string OrderDescription { get; set; }

        public int? OrderNumber { get; set; }

        public decimal OrderValue { get; set; }

        public int POLineNumber { get; set; }

        public int Qty { get; set; }

        public int? RsnNumber { get; set; }

        public string TariffCode { get; set; }

        public int? VatRate { get; set; }

        public decimal VatValue { get; set; }

        public decimal Weight { get; set; }

        public string PostDuty { get; set; }

        public ImportBookCpcNumber ImportBookCpcNumber { get; set; }

        public int? LineDocument()
        {
            if (this.LineType == "RSN")
            {
                return this.RsnNumber;
            }

            if (this.LineType == "PO" || this.LineType == "RO")
            {
                return this.OrderNumber;
            }

            if (this.LineType == "LOAN")
            {
                return this.LoanNumber;
            }

            return null;
        }
    }
}
