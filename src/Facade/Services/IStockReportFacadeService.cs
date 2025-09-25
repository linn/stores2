namespace Linn.Stores2.Facade.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Resources;

    public interface IStockReportFacadeService
    {
        Task<IResult<ReportReturnResource>> LabourHoursInStock(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<IResult<TotalResource>> LabourHoursInStockTotal(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<string> LabourHoursInStockAsHtml(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<Stream> LabourHoursInStockAsPdf(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<IResult<ReportReturnResource>> LabourHourSummary(
            string fromDate,
            string toDate,
            string accountingCompany = "LINN",
            bool recalcLabourTimes = false);
    }
}
