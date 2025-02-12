namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class GoodsInLogReportFacadeService : IGoodsInLogReportFacadeService
    {
        private readonly IGoodsInLogReportService goodsInLogReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public GoodsInLogReportFacadeService(
            IGoodsInLogReportService goodsInLogReportService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.goodsInLogReportService = goodsInLogReportService;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<ReportReturnResource>> GetGoodsInLogReport(
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace)
        {
            var result = await this.goodsInLogReportService.GoodsInLogReport(
                fromDate,
                toDate,
                createdBy,
                articleNumber,
                quantity,
                orderNumber,
                reqNumber,
                storagePlace);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }
    }
}
