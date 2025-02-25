namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IStoresTransViewerReportFacadeService
    {
        IResult<ReportReturnResource> GetStoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList);
    }
}