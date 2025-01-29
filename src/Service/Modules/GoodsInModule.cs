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
            app.MapGet("/stores2/goods-in/log", this.SearchLog);
        }

        private async Task SearchLog(
            HttpRequest _,
            HttpResponse res,
            string dateCreated,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace,
            IAsyncFacadeService<GoodsInLogEntryResource, int, GoodsInLogEntryResource, GoodsInLogEntryResource, GoodsInLogEntrySearchResource> goodsInLogFacadeService)
        {
            if (string.IsNullOrWhiteSpace(dateCreated) && !createdBy.HasValue && 
                string.IsNullOrWhiteSpace(articleNumber) && !quantity.HasValue && 
                !orderNumber.HasValue && !reqNumber.HasValue &&
                string.IsNullOrWhiteSpace(articleNumber))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                await res.Negotiate(await goodsInLogFacadeService.FilterBy(
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
}
