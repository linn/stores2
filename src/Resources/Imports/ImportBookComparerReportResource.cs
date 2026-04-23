namespace Linn.Stores2.Resources.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ImportBookComparerReportResource
    {
        public string EntryId { get; set; }

        public DateTime ClearanceDate { get; set; }

        public string Consignor { get; set; }

        public string CountryOfDispatch { get; set; }

        public int CommodityCode { get; set; }

        public string Cpc { get; set; }

        public string CountryOfOrigin { get; set; }

        public string InvoiceCurrency { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal CustomsValue { get; set; }

        public decimal VatValue { get; set; }
    }
}
