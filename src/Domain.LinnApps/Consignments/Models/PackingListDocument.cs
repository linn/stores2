namespace Linn.Stores2.Domain.LinnApps.Consignments.Models
{
    using System;

    public class PackingListDocument
    {
        public Consignment Consignment { get; set; }

        public string SenderAddress { get; set; } =
            $"Linn Products Ltd{Environment.NewLine}Glasgow Road{Environment.NewLine}Waterfoot{Environment.NewLine}Glasgow{Environment.NewLine}G76 0EQ{Environment.NewLine}United Kingdom";

        public string CarrierReference { get; set; }
    }
}
