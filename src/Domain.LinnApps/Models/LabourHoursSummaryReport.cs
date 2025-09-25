namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;
    using Linn.Common.Reporting.Models;

    public class LabourHoursSummaryReport
    {
        public ResultsModel Report { get; set; }

        public ResultsModel TotalsReport { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string AccountingCompany { get; set; }
    }
}
