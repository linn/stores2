namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;

    public class ImportClearanceInstructionDetail
    {
        public string InvoiceNumber { get; set; }

        public string Description { get; set; }

        public string TariffCode { get; set; }

        public string CountryOfOrigin { get; set; }

        public decimal? CustomsValue { get; set; }

        public Currency Currency { get; set; }

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
