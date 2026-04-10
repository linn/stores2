namespace Linn.Stores2.Resources.Imports
{
    using System;

    public class ImportBookSearchResource
    {
        public string TransportBillNumber { get; set; }

        public string CustomsEntryCode { get; set; }

        public int? RsnNumber { get; set; }

        public int? PONumber { get; set; }

        public string DateField { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }
}
