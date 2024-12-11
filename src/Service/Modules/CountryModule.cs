namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class CountryModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/countries/{countryCode}", this.GetCountry);
        }

        private async Task GetCountry(HttpRequest req, HttpResponse res, ICountryFacadeService service, string countryCode)
        {
            await res.Negotiate(await service.GetCountry(countryCode));
        }
    }
}
