namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class CarrierModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/carriers", this.Search);
            app.MapPost("/stores2/carriers", this.Create);
            app.MapGet("/stores2/carriers/{code}", this.GetById);
            app.MapPut("/stores2/carriers/{code}", this.Update);
        }

        private async Task Search(
            HttpResponse res,
            string searchTerm,
            IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource> service)
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
            HttpResponse res,
            string code,
            IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource> service)
        {
            await res.Negotiate(await service.GetById(code));
        }

        private async Task Create(
            HttpResponse res,
            CarrierResource resource,
            IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource> service)
        {
            await res.Negotiate(await service.Add(resource));
        }

        private async Task Update(
            HttpResponse res,
            string code,
            CarrierUpdateResource resource,
            IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource> service)
        {
            await res.Negotiate(await service.Update(code, resource));
        }
    }
}
