namespace Linn.Stores2.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class StoresTransViewerReportFacadeService : IStoresTransViewerReportFacadeService
    {
        private readonly IStoresTransViewerReportService storesTransViewerReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public StoresTransViewerReportFacadeService(
            IStoresTransViewerReportService storesTransViewerReportService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.storesTransViewerReportService = storesTransViewerReportService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetStoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            string functionCode)
        {
            var result = this.storesTransViewerReportService.StoresTransViewerReport(
                fromDate,
                toDate,
                partNumber, 
                transactionCode,
                functionCode);

            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }
    }
}
