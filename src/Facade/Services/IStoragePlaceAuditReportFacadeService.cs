namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Resources;

    public interface IStoragePlaceAuditReportFacadeService
    {
        IResult<ReportReturnResource> GetStoragePlaceAuditReport(
            IEnumerable<string> locationList,
            string locationRange);

        Task<Stream> GetStoragePlaceAuditReportAsPdf(string[] locationList, string locationRange);

        Task<IResult<ProcessResultResource>> CreateCheckedAuditReqs(
            string[] locationList,
            string locationRange,
            int employeeNumber,
            string departmentCode,
            IEnumerable<string> privileges);
    }
}
