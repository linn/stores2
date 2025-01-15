namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IStoragePlaceAuditReportFacadeService
    {
        IResult<ReportReturnResource> GetStoragePlaceAuditReport(
            IEnumerable<string> locationList,
            string locationRange);

        Task<Stream> GetStoragePlaceAuditReportAsPdf(string[] locationList, string locationRange);
    }
}
