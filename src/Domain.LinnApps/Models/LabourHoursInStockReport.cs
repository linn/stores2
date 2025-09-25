namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;
    using Linn.Common.Reporting.Models;

    public class LabourHoursInStockReport
    {
        public LabourHoursInStockReport(ResultsModel report, decimal hoursTotal)
        {
            this.Report = report;
            this.ReportDate = DateTime.Now;
            HoursTotal = hoursTotal;
        }

        public ResultsModel Report { get; set; }

        public DateTime ReportDate { get; set; }

        public decimal HoursTotal { get; set; }
    }
}
