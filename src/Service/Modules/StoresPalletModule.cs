namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StoresPalletModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/pallets", this.Search);
            app.MapPost("/stores2/pallets", this.Create);
            app.MapGet("/stores2/pallets/{id}", this.GetById);
            app.MapPut("/stores2/pallets/{id}", this.Update);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string searchTerm,
            IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource> service)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                await res.Negotiate(await service.GetAll());
            }
            else
            {
                await res.Negotiate(await service.Search(searchTerm));
            }
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int id,
            IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource> service)
        {
            await res.Negotiate(await service.GetById(id));
        }

        private async Task Create(
            HttpRequest _,
            HttpResponse res,
            StoresPalletResource resource,
            IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpRequest _,
            HttpResponse res,
            int id,
            StoresPalletResource resource,
            IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource> service)
        {
            await res.Negotiate(await service.Update(id, resource));
        }
    }
}
