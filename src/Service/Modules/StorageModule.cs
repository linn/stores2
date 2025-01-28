namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Service.Core;
    using Linn.Stores2.Service.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StorageModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/storage", this.GetApp);
            app.MapGet("/stores2/storage/locations", this.SearchLocations);
            app.MapGet("/stores2/storage/locations/{id:int}", this.GetLocationById);
            app.MapGet("/stores2/storage/sites", this.GetSites);
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
    }
}
