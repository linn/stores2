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

    public class PartsStorageTypeModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/parts-storage-types", this.GetAll);
            app.MapPost("/stores2/parts-storage-types", this.Create);
            app.MapGet("/stores2/parts-storage-types/{bridgeId}", this.GetById);
            app.MapPut("/stores2/parts-storage-types/{bridgeId}", this.Update);
        }

        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            string part,
            string storageType,
            IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            if (string.IsNullOrEmpty(part) && string.IsNullOrEmpty(storageType))
            {
                await res.Negotiate(await service.GetAll());
            }
            else
            {
                var searchResource = new PartsStorageTypeResource
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
            IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            var searchResource = new PartsStorageTypeResource
                          {
                              BridgeId = bridgeId,
                          };

            await res.Negotiate(await service.GetById(bridgeId));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            PartsStorageTypeResource resource,
            IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            int bridgeId,
            PartsStorageTypeResource resource,
            IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            await res.Negotiate(await service.Update(bridgeId, resource));
        }
    }
}
