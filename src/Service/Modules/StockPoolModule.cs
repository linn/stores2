namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
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
            app.MapGet("/stores2/stock-pools/{code}", this.GetStockPool);
            app.MapPost("/stores2/stock-pools", this.CreateStockPool);
            app.MapPut("/stores2/stock-pools/{code}", this.UpdateStockPool);
        }

        private async Task GetStockPools(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource> facadeService)
        {
            var results = await facadeService.GetAll(req.HttpContext.GetPrivileges());
            await res.Negotiate(results);
        }

        private async Task GetStockPool(
            HttpRequest req,
            HttpResponse res,
            string code,
            IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource> facadeService)
        {
            var results = await facadeService.GetById(code, req.HttpContext.GetPrivileges());
            await res.Negotiate(results);
        }

        private async Task CreateStockPool(
            HttpRequest req,
            HttpResponse res,
            StockPoolResource resource,
            IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource> facadeService)
        {
            await res.Negotiate(await facadeService.Add(resource, req.HttpContext.GetPrivileges()));
        }

        private async Task UpdateStockPool(
            HttpRequest req,
            HttpResponse res,
            string code,
            StockPoolUpdateResource resource,
            IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource> facadeService)
        {
            await res.Negotiate(await facadeService.Update(code, resource, req.HttpContext.GetPrivileges()));
        }
    }
}
