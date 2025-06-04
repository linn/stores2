namespace Linn.Stores2.Facade.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IRequisitionReportFacadeService
    {
        Task<IResult<ReportReturnResource>> GetRequisitionCostReport(int reqNumber);

        Task<Stream> GetRequisitionCostReportAsPdf(int reqNumber);
    }
}
