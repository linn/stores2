namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;

    using Linn.Common.Reporting.Models;

    public class LabourHoursInLoansReport
    {
        public ResultsModel Report { get; set; }

        public DateTime ReportDate { get; set; }

        public decimal HoursTotal { get; set; }
    }
}
