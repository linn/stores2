using Linn.Common.Facade;
using Linn.Common.Pdf;
using Linn.Common.Reporting.Resources.ReportResultResources;
using Linn.Common.Reporting.Resources.ResourceBuilders;
using Linn.Stores2.Domain.LinnApps.Requisitions;
using System;
using System.Threading.Tasks;

namespace Linn.Stores2.Facade.Services
{
    internal class DailyEuReportsFacadeService : IDailyEuReportFacdeService
    {
        public RequisitionReportFacadeService(
            IRequisitionReportService requisitionReportService,
            IReportReturnResourceBuilder reportResourceBuilder,
            IPdfService pdfService)
        {
            this.requisitionReportService = requisitionReportService;
            this.reportResourceBuilder = reportResourceBuilder;
            this.pdfService = pdfService;
        }

        public Task<IResult<ReportReturnResource>> GetDailyEuDespatchReport(string fromDate, string toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<ReportReturnResource>> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            throw new NotImplementedException();
        }
    }
}
