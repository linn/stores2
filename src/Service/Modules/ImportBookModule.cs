namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.Service.Extensions;
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
            app.MapPost("/stores2/import-books", this.Create);
        }

        private async Task SearchImports(
            HttpRequest _,
            HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IAsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource> service)
        {
            var privs = req.HttpContext.GetPrivileges();

            await res.Negotiate(await service.GetById(id, req.HttpContext.GetPrivileges()));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task Create(
            HttpRequest req,
            HttpResponse res,
            ImportBookResource resource,
            IAsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource> service)
        {
            if (resource.CreatedById == null)
            {
                resource.CreatedById = req.HttpContext.User.GetEmployeeNumber();
            }

            await res.Negotiate(await service.Add(resource));
        }
    }
}
