namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class RequisitionReportModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/requisitions/reports/requisition-cost/report", this.RequisitionCostReport);
            app.MapGet("/requisitions/reports/requisition-cost", this.GetApp);
            app.MapGet("/requisitions/reports/requisition-cost/pdf", this.RequisitionCostReportAsPdf);
        }

        private async Task RequisitionCostReport(
            HttpRequest _, 
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.GetRequisitionCostReport(reqNumber));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task RequisitionCostReportAsPdf(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            var result = await facadeService.GetRequisitionCostReportAsPdf(reqNumber);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }
    }
}
