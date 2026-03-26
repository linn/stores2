namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;

    public class ImportClearanceInstructionTotal
    {
        public Currency Currency { get; set; }

        public decimal TotalValue { get; set; }

        public override string ToString()
        {
            var formattedValue = this.Currency?.Code == "JPY"
                ? this.TotalValue.ToString("N0")
                : this.TotalValue.ToString("N2");

            return $"{this.Currency?.Code} {formattedValue}";
        }
    }
}
