namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;

    using Linn.Common.Reporting.Models;

    public class StoragePlaceAuditReport
    {
        public StoragePlaceAuditReport(ResultsModel report)
        {
            this.Report = report;
            this.ReportDate = DateTime.Now;
        }

        public ResultsModel Report { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
