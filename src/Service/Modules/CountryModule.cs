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

    public class CountryModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/countries", this.GetCountries);
        }

        private async Task GetCountries(
            HttpRequest req, 
            HttpResponse res, 
            IAsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }
    }
}
