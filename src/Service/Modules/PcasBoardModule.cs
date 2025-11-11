namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Resources.Pcas;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PcasBoardModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/pcas-boards", this.Search);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string searchTerm,
            IAsyncFacadeService<PcasBoard, string, PcasBoardResource, PcasBoardResource, PcasBoardResource> service)
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
    }
}
