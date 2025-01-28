namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StorageTypeModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/storage-types", this.GetAll);
            app.MapPost("/stores2/storage-types", this.Create);
            app.MapGet("/stores2/storage-types/{code}", this.GetById);
            app.MapPut("/stores2/storage-types/{code}", this.Update);
        }
        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            string code,
            IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource> service)
        {
            await res.Negotiate(await service.GetById(code));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            StorageTypeResource resource,
            IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            string code,
            StorageTypeResource resource,
            IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource> service)
        {
            await res.Negotiate(await service.Update(code, resource));
        }
    }
}
