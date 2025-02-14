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

    public class GoodsInModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/goods-in-log/report", this.GoodsInLogReport);
            app.MapGet("/stores2/goods-in-log", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GoodsInLogReport(
            HttpRequest _,
            HttpResponse res,
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace,
            IGoodsInLogReportFacadeService facadeService)
        {
            await res.Negotiate(facadeService.GetGoodsInLogReport(
                fromDate,
                toDate,
                createdBy,
                articleNumber,
                quantity,
                orderNumber,
                reqNumber,
                storagePlace));
        }
    }
}
