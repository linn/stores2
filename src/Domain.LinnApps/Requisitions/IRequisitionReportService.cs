namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;

    public interface IRequisitionReportService
    {
        Task<ResultsModel> GetRequisitionCostReport(int reqNumber);

        Task<string> GetRequisitionCostReportAsHtml(int reqNumber);

        Task<string> GetRequisitionAsHtml(int reqNumber);
    }
}
