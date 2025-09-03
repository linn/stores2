namespace Linn.Stores2.Service.Modules
{
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class DailyEuReportsModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/customs/daily/eu/import/rsn/report", this.DailyEuImportRsnReport);
            app.MapGet("/stores2/customs/daily/eu/despatch/report", this.DailyEuDespatchReport);
            app.MapGet("/requisitions/reports/requisition-cost", this.GetApp);
        }

        private async Task DailyEuImportRsnReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            IDailyEuReportFacdeService facadeService)
        {
            await res.Negotiate(facadeService.GetDailyEuImportRsnReport(fromDate, toDate));
        }

        private async Task DailyEuDespatchReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            IDailyEuReportFacdeService facadeService)
        {
            await res.Negotiate(facadeService.GetDailyEuDespatchReport(fromDate, toDate));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
