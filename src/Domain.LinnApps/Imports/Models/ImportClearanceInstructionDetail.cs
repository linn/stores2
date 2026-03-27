namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;

    public class ImportClearanceInstructionDetail
    {
        public ImportClearanceInstructionDetail(ImportBookOrderDetail order, ImportBookInvoiceDetail invoice, Currency currency)
        {
            this.InvoiceNumber = invoice?.InvoiceNumber;
            this.Description = order?.OrderDescription;
            this.TariffCode = order?.TariffCode;
            this.CountryOfOrigin = order?.CountryOfOrigin;
            this.CustomsValue = invoice?.InvoiceValue;
            this.Currency = currency;
        }

        public string InvoiceNumber { get; set; }

        public string Description { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public decimal? CustomsValue { get; set; }

        public Currency Currency { get; set; }

        public string ExportInvoice { get; set; }

        public string ExportEntryNumber { get; set; }

        public string ExportEntryDate { get; set; }

        public string CustomsValueString()
        {
            if (this.Currency != null && this.CustomsValue.HasValue)
            {
                if (this.Currency.Code == "JPY")
                {
                    return $"JPY {this.CustomsValue.Value:N0}";
                }

                return $"{this.Currency?.Code} {this.CustomsValue.Value:N2}";
            }

            return null;
        }
    }
}
