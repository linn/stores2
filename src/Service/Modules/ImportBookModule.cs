namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ImportBookModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/import-books", this.SearchImports);
            app.MapGet("/stores2/import-books/{id:int}", this.GetById);
            app.MapGet("/stores2/import-books/create", this.GetApp);
        }

        private async Task SearchImports(
            HttpRequest _,
            HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int id,
            IAsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource> service)
        {
            await res.Negotiate(await service.GetById(id));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
