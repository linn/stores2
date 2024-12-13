namespace Linn.Stores2.Service.Modules
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;
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
            HttpRequest _, 
            HttpResponse res,
            string searchTerm,
            ICarrierService service)
        {
            if (String.IsNullOrEmpty(searchTerm))
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
            string code,
            ICarrierService service)
        {
            await res.Negotiate(await service.GetById(code));
        }
        
        private async Task Create(
            HttpRequest _, 
            HttpResponse res, 
            CarrierResource resource,
            ICarrierService service)
        {
            await res.Negotiate(await service.Create(resource));
        }
        
        private async Task Update(
            HttpRequest _, 
            HttpResponse res, 
            string code,
            CarrierUpdateResource resource,
            ICarrierService service)
        {
            await res.Negotiate(await service.Update(code, resource));
        }
    }
}
