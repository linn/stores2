namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources.Parts;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PartStorageTypeModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/parts-storage-types", this.GetAll);
            app.MapPost("/stores2/parts-storage-types", this.Create);
            app.MapGet("/stores2/parts-storage-types/{bridgeId}", this.GetById);
            app.MapPut("/stores2/parts-storage-types/{bridgeId}", this.Update);
            app.MapDelete("/stores2/parts-storage-types/{bridgeId}", this.Delete);
        }

        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            string part,
            string storageType,
            IAsyncFacadeService<PartStorageType, int, PartStorageTypeResource, PartStorageTypeResource, PartStorageTypeResource> service)
        {
            if (string.IsNullOrEmpty(part) && string.IsNullOrEmpty(storageType))
            {
                await res.Negotiate(await service.GetAll());
            }
            else
            {
                var searchResource = new PartStorageTypeResource
                                         {
                                             PartNumber = part,
                                             StorageTypeCode = storageType,
                                         };

                await res.Negotiate(await service.FilterBy(searchResource));
            }
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int bridgeId,
            IAsyncFacadeService<PartStorageType, int, PartStorageTypeResource, PartStorageTypeResource, PartStorageTypeResource> service)
        {
            await res.Negotiate(await service.GetById(bridgeId));
        }

        private async Task Delete(
            HttpRequest _,
            HttpResponse res,
            int bridgeId,
            IAsyncFacadeService<PartStorageType, int, PartStorageTypeResource, PartStorageTypeResource, PartStorageTypeResource> service)
        {
            await res.Negotiate(await service.DeleteOrObsolete(bridgeId));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            PartStorageTypeResource resource,
            IAsyncFacadeService<PartStorageType, int, PartStorageTypeResource, PartStorageTypeResource, PartStorageTypeResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            int bridgeId,
            PartStorageTypeResource resource,
            IAsyncFacadeService<PartStorageType, int, PartStorageTypeResource, PartStorageTypeResource, PartStorageTypeResource> service)
        {
            await res.Negotiate(await service.Update(bridgeId, resource));
        }
    }
}
