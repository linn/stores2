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

    public class StockTransactionModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/stores-trans-viewer/report", this.StoresTransViewerReport);
            app.MapGet("/stores2/stores-trans-viewer/pdf", this.GetStoresTransactionReportPdf);
            app.MapGet("/stores2/stores-trans-viewer", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task StoresTransViewerReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            string[] functionCodeList,
            IStoresTransViewerReportFacadeService facadeService)
        {
            await res.Negotiate(await facadeService.GetStoresTransViewerReport(
                fromDate,
                toDate,
                partNumber,
                transactionCode,
                functionCodeList));
        }

        private async Task GetStoresTransactionReportPdf(
            HttpRequest req,
            HttpResponse res,
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            string[] functionCodeList,
            IStoresTransViewerReportFacadeService facadeService)
        {
            var result = await facadeService.GetStoresTransactionReportAsPdf(
                             fromDate,
                             toDate,
                             partNumber,
                             transactionCode,
                             functionCodeList);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }
    }
}
