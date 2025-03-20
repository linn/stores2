namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IStoresTransViewerReportFacadeService
    {
        Task<IResult<ReportReturnResource>> GetStoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList);

        Task<Stream> GetStoresTransactionReportAsPdf(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList);
    }
}