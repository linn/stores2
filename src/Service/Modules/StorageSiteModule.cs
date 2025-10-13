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

    public class StorageSiteModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/storage/sites", GetStorageSites);
            app.MapPost("/stores2/storage/sites", Create);
            app.MapGet("/stores2/storage/sites/{siteCode}", GetById);
            app.MapPut("/stores2/storage/sites/{siteCode}", Update);
        }

        private static async Task GetStorageSites(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private static async Task GetById(
            HttpRequest req,
            HttpResponse res,
            string siteCode,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.GetById(siteCode, req.HttpContext.GetPrivileges()));
        }

        private static async Task Update(
            HttpRequest req,
            HttpResponse res,
            string siteCode,
            StorageSiteResource resource,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.Update(siteCode, resource, req.HttpContext.GetPrivileges()));
        }

        private static async Task Create(
            HttpRequest req,
            HttpResponse res,
            StorageSiteResource resource,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges()));
        }
    }
}
