namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;
    using System.Threading.Tasks;
    using Linn.Common.Reporting.Models;

    public class StoresTransactionReport
    {
        public StoresTransactionReport(ResultsModel report)
        {
            this.Report = report;
        }

        public ResultsModel Report { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
