namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using Linn.Stores2.Domain.LinnApps.Parts;

    public class TqmsData
    {
        public string Jobref { get; set; }

        public string PartNumber { get; set; }

        public Part Part { get; set; }

        public string TqmsCategoryCode { get; set; }

        public decimal CurrentUnitPrice { get; set; }

        public decimal TotalQty { get; set; }

        public decimal LabourHours()
        {
            if (this.Part != null && this.Part.Bom != null)
            {
                return this.Part.Bom.TotalLabourTimeHours * this.TotalQty;
            }

            return 0;
        }
    }
}
