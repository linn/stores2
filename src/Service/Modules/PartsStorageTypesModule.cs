namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PartsStorageTypesModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/parts-storage-types", this.GetAll);
            app.MapPost("/stores2/parts-storage-types", this.Create);
            app.MapGet("/stores2/parts-storage-types/{partNumber}/{storageTypeCode}", this.GetById);
            app.MapPut("/stores2/parts-storage-types/{partNumber}/{storageTypeCode}", this.Update);
        }

        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            string partNumber,
            string storageTypeCode,
            IAsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            var searchResource = new PartsStorageTypeResource
                          {
                              PartNumber = partNumber,
                              StorageTypeCode = storageTypeCode
                          };

            await res.Negotiate(await service.FindBy(searchResource));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            PartsStorageTypeResource resource,
            IAsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            string PartNumber,
            string StorageTypeCode,
            PartsStorageTypeResource resource,
            IAsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource> service)
        {
            var key = new PartsStorageTypeKey(partNumber: PartNumber, storageTypeCode: StorageTypeCode);

            await res.Negotiate(await service.Update(key, resource));
        }
    }
}
