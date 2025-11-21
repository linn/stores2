namespace Linn.Stores2.Service.Modules
{
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
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
            app.MapGet("/requisitions/reports/requisition-cost/report/{reqNumber}/view", this.RequisitionCostReportAsHtml);
            app.MapGet("/requisitions/reports/requisition-cost/report/{reqNumber}/pdf", this.RequisitionCostReportAsPdf);
            app.MapGet("/requisitions/reports/requisition-cost", this.GetApp);
            app.MapGet("/requisitions/{reqNumber}/view", this.GetReqAsHtml);
            app.MapGet("/requisitions/{reqNumber}/pdf", this.GetReqAsPdf);
        }

        private async Task RequisitionCostReport(
            HttpRequest _, 
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.GetRequisitionCostReport(reqNumber));
        }

        private async Task RequisitionCostReportAsHtml(
            HttpRequest _,
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
           var result = await facadeService.GetRequisitionCostReportAsHtml(reqNumber);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task RequisitionCostReportAsPdf(
            HttpRequest _,
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            var result = await facadeService.GetRequisitionCostReportAsPdf(reqNumber);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetReqAsHtml(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            var result = await facadeService.GetRequisitionAsHtml(reqNumber);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task GetReqAsPdf(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IRequisitionReportFacadeService facadeService)
        {
            var result = await facadeService.GetRequisitionAsPdf(reqNumber);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }
    }
}
