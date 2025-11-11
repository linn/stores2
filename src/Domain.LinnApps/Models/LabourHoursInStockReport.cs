namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;
    using Linn.Common.Reporting.Models;

    public class LabourHoursInStockReport
    {
        public LabourHoursInStockReport(ResultsModel report, decimal hoursTotal, string jobref)
        {
            this.Report = report;
            this.ReportDate = DateTime.Now;
            this.HoursTotal = hoursTotal;
            this.Jobref = jobref;
        }

        public ResultsModel Report { get; set; }

        public string Jobref { get; set; }

        public DateTime ReportDate { get; set; }

        public decimal HoursTotal { get; set; }
    }
}
