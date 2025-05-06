namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Pcas;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PcasStorageTypeModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/pcas-storage-types", this.GetAll);
            app.MapPost("/stores2/pcas-storage-types", this.Create);
            app.MapGet("/stores2/pcas-storage-types/{boardCode}/{storageTypeCode}", this.GetById);
            app.MapPut("/stores2/pcas-storage-types/{boardCode}/{storageTypeCode}", this.Update);
        }

        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            string boardCode,
            string storageTypeCode,
            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> service)
        {
            var searchResource = new PcasStorageTypeResource { BoardCode = boardCode, StorageTypeCode = storageTypeCode};
            await res.Negotiate(await service.FindBy(searchResource));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            PcasStorageTypeResource resource,
            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            string boardCode,
            string storageTypeCode,
            PcasStorageTypeResource resource,
            IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource> service)
        {
            var key = new PcasStorageTypeKey { BoardCode = boardCode, StorageTypeCode = storageTypeCode };

            await res.Negotiate(await service.Update(key, resource));
        }
    }
}
