namespace Linn.Stores2.Domain.LinnApps.External
{
    using System;

    public class LedgerPeriodResult
    {
        public int PeriodNumber { get; set; }

        public string MonthName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ReportName { get; set; }
    }
}
