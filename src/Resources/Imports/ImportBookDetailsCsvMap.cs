namespace Linn.Stores2.Resources.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CsvHelper.Configuration;

    public class ImportBookDetailsCsvMap : ClassMap<ImportBookComparerReportResource>
    {
        public ImportBookDetailsCsvMap()
        {
            this.MapFields();
        }

        public void MapFields()
        {
            this.Map(m => m.EntryId).Name("Entry Identifier");
            this.Map(m => m.ClearanceDate).Name("Clearance Date");
            this.Map(m => m.Consignor).Name("Consignor");
            this.Map(m => m.CountryOfDispatch).Name("Country of Dispatch");
            this.Map(m => m.CommodityCode).Name("Commodity Code");
            this.Map(m => m.Cpc).Name("CPC");
            this.Map(m => m.CountryOfOrigin).Name("Country of Origin");
            this.Map(m => m.InvoiceCurrency).Name("Invoice Currency");
            this.Map(m => m.ItemPrice).Name("Item Price");
            this.Map(m => m.CustomsValue).Name("Customs Value");
            this.Map(m => m.VatValue).Name("VAT Value");
        }
    }
}
