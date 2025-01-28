namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.GoodsIn;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class GoodsInModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/logistics", this.GetApp);
            app.MapGet("/stores2/logistics/goods-in/log", this.SearchLog);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task SearchLog(
            HttpRequest _,
            HttpResponse res,
            string dateCreated,
            int createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace,
            IAsyncFacadeService<GoodsInLogEntryResource, int, GoodsInLogEntryResource, GoodsInLogEntryResource, GoodsInLogEntrySearchResource> goodsInLogFacadeService)
        {
            await res.Negotiate(goodsInLogFacadeService.FilterBy(
                new GoodsInLogEntrySearchResource
                {
                    CreatedBy = createdBy,
                    ArticleNumber = articleNumber,
                    Quantity = quantity,
                    OrderNumber = orderNumber,
                    ReqNumber = reqNumber,
                    StoragePlace = storagePlace,
                    DateCreated = dateCreated
                }));
        }
    }
}
