namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Stores2.Resources;

    public interface IStockReportFacadeService
    {
        Task<IResult<ReportReturnResource>> LabourHoursInStock(string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<IResult<TotalResource>> LabourHoursInStockTotal(string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);
    }
}
