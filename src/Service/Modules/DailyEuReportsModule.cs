namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class DailyEuReportsModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/reports/daily-eu-import-rsn", this.DailyEuImportRsnReport);
            app.MapGet("/stores2/reports/daily-eu-dispatch", this.DailyEuDispatchReport);
            app.MapGet("/stores2/reports/daily-eu-dispatch-rsn", this.DailyEuDispatchRsnReport);
        }

        private async Task DailyEuImportRsnReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            IDailyEuReportFacadeService facadeService)
        {
            if (string.IsNullOrEmpty(fromDate))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                await res.Negotiate(await facadeService.GetDailyEuImportRsnReport(fromDate, toDate));
            }
        }

        private async Task DailyEuDispatchReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            IDailyEuReportFacadeService facadeService)
        {
            if (string.IsNullOrEmpty(fromDate))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                await res.Negotiate(await facadeService.GetDailyEuDespatchReport(fromDate, toDate));
            }
        }

        private async Task DailyEuDispatchRsnReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            IDailyEuReportFacadeService facadeService)
        {
            if (string.IsNullOrEmpty(fromDate))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                await res.Negotiate(await facadeService.GetDailyEuDespatchRsnReport(fromDate, toDate));
            }
        }
    }
}
