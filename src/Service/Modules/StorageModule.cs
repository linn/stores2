using Linn.Common.Service.Core.Extensions;
using Linn.Stores2.Domain.LinnApps;
using Linn.Stores2.Domain.LinnApps.Stock;
using Linn.Stores2.Facade.Common;
using Linn.Stores2.Resources;

namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Service.Core;
    using Linn.Stores2.Service.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StorageModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/storage", this.GetApp);
            app.MapGet("/stores2/storage/sites", this.GetSites);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetSites(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }
    }
}
