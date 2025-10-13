namespace Linn.Stores2.Domain.LinnApps.Models
{
    using Linn.Common.Reporting.Models;
    using System;

    public class LabourHoursInLoansReport
    {
        public ResultsModel Report { get; set; }

        public DateTime ReportDate { get; set; }

        public decimal HoursTotal { get; set; }
    }
}
