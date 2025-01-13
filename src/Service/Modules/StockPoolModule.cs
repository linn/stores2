namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StockPoolModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/stock-pools", this.GetStockPools);
        }

        private async Task GetStockPools(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<StockPool, string, StockPoolResource, StockPoolResource> service)
        {
            await res.Negotiate(service.GetAll(req.HttpContext.GetPrivileges()));
        }
    }
}
