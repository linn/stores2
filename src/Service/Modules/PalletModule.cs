namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Pcas;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PalletModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/pallets", this.GetAll);
            app.MapPost("/stores2/pallets", this.Create);
            app.MapGet("/stores2/pallets/{id}", this.GetById);
            app.MapPut("/stores2/pallets/{id}", this.Update);
        }

        private async Task GetAll(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<Pallet, int, PalletResource, PalletResource, PalletResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int id,
            IAsyncFacadeService<Pallet, int, PalletResource, PalletResource, PalletResource> service)
        {
            await res.Negotiate(await service.GetById(id));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            PalletResource resource,
            IAsyncFacadeService<Pallet, int, PalletResource, PalletResource, PalletResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            int id,
            PalletResource resource,
            IAsyncFacadeService<Pallet, int, PalletResource, PalletResource, PalletResource> service)
        {
            await res.Negotiate(await service.Update(id, resource));
        }
    }
}