namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Service.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    public class StockReportModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/reports/labour-hours-in-stock/report", this.GetLabourHoursInStock);
            app.MapGet("/stores2/reports/labour-hours-in-stock/total", this.GetLabourHoursInStockTotal);
            app.MapGet("/stores2/reports/labour-hours-in-stock", this.GetApp);
            app.MapGet("/stores2/reports/labour-hours-summary", this.GetApp);
            app.MapGet("/stores2/reports/labour-hours-summary/report", this.GetLabourHourSummaries);
        }

        private async Task GetLabourHoursInStock(
            HttpRequest _,
            HttpResponse res,
            string accountingCompany,
            string jobref,
            IStockReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.LabourHoursInStock(jobref, accountingCompany, true));
        }

        private async Task GetLabourHoursInStockTotal(
            HttpRequest _,
            HttpResponse res,
            string accountingCompany,
            string jobref,
            IStockReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.LabourHoursInStockTotal(jobref, accountingCompany, true));
        }

        private async Task GetLabourHourSummaries(
            HttpRequest _,
            HttpResponse res,
            string accountingCompany,
            string fromDate,
            string toDate,
            IStockReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.LabourHourSummary(fromDate, toDate, accountingCompany));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
