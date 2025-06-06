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
    }
}
