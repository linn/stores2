namespace Linn.Stores2.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IGoodsInLogReportFacadeService
    {
        IResult<ReportReturnResource> GetGoodsInLogReport(
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace);
    }
}
