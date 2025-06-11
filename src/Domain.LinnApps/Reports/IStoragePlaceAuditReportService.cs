namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Reporting.Models;

    public interface IStoragePlaceAuditReportService
    {
        ResultsModel StoragePlaceAuditReport(IEnumerable<string> locationList, string locationRange);

        Task<ProcessResult> CreateSuccessAuditReqs(
            int employeeNumber,
            IEnumerable<string> locationList,
            string locationRange,
            string departmentCode,
            IList<string> privileges);
    }
}
