namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public class StoragePlaceAuditReportFacadeService : IStoragePlaceAuditReportFacadeService
    {
        private readonly IStoragePlaceAuditReportService storagePlaceAuditReportService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public StoragePlaceAuditReportFacadeService(
            IStoragePlaceAuditReportService storagePlaceAuditReportService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.storagePlaceAuditReportService = storagePlaceAuditReportService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetStoragePlaceAuditReport(IEnumerable<string> locationList, string locationRange)
        {
            var result = this.storagePlaceAuditReportService.StoragePlaceAuditReport(locationList, locationRange);
            
            return new SuccessResult<ReportReturnResource>(this.resourceBuilder.Build(result));
        }
    }
}
