namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Extensions;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StorageModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/storage", this.GetApp);
            app.MapGet("/stores2/storage/locations", this.SearchLocations);
            app.MapGet("/stores2/storage/locations/{id:int}", this.GetLocationById);
            app.MapGet("/stores2/storage/sites", this.GetSites);
            app.MapPost("/stores2/storage/locations", this.CreateLocation);
            app.MapPut("/stores2/storage/locations/{id:int}", this.UpdateLocation);
            app.MapGet("/stores2/stock/states", this.GetStockStates);
            app.MapGet("/stores2/stock/states/{id}", this.GetStockState);
            app.MapGet("/stores2/storage/audit-locations", this.GetAuditLocations);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task SearchLocations(
            HttpRequest _,
            HttpResponse res,
            string searchTerm,
            string siteCode,
            string storageAreaCode,
            IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource> service)
        {
            if (string.IsNullOrEmpty(searchTerm) && string.IsNullOrEmpty(siteCode) && string.IsNullOrEmpty(storageAreaCode))
            {
                await res.Negotiate(await service.GetAll());
            }
            else if (string.IsNullOrEmpty(siteCode) && string.IsNullOrEmpty(storageAreaCode))
            {
                await res.Negotiate(await service.Search(searchTerm));
            }
            else
            {
                var searchResource = new StorageLocationResource()
                {
                    LocationCode = searchTerm,
                    StorageAreaCode = storageAreaCode,
                    SiteCode = siteCode
                };
                await res.Negotiate(await service.FilterBy(searchResource));
            }
        }

        private async Task GetAuditLocations(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IAsyncQueryFacadeService<AuditLocation, AuditLocationResource, AuditLocationResource> facadeService)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                await res.Negotiate(
                    await facadeService.FilterBy(new AuditLocationResource { StoragePlace = searchTerm.ToUpper() }));
            }
            else
            {
                await res.Negotiate(await facadeService.GetAll());
            }
        }

        private async Task GetLocationById(
            HttpRequest _,
            HttpResponse res,
            int id,
            IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource> service)
        {
            await res.Negotiate(await service.GetById(id));
        }

        private async Task GetSites(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task CreateLocation(
            HttpRequest req,
            HttpResponse res,
            StorageLocationResource resource,
            IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource> service)
        {
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges()));
        }

        private async Task UpdateLocation(
            HttpRequest req,
            HttpResponse res,
            int id,
            StorageLocationResource resource,
            IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource> facadeService)
        {
            await res.Negotiate(await facadeService.Update(id, resource, req.HttpContext.GetPrivileges()));
        }

        private async Task GetStockStates(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<StockState, string, StockStateResource, StockStateResource, StockStateResource> facadeService)
        {
            await res.Negotiate(await facadeService.GetAll());
        }

        private async Task GetStockState(
            HttpRequest _,
            HttpResponse res,
            string id,
            IAsyncFacadeService<StockState, string, StockStateResource, StockStateResource, StockStateResource> facadeService)
        {
            await res.Negotiate(await facadeService.GetById(id));
        }
    }
}
