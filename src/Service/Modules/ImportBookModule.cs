namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;
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
            app.MapPost("/stores2/import-books/{id:int}", this.Update);
            app.MapGet("/stores2/import-books/initialise", this.Initialise);
        }

        private async Task SearchImports(
            HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task GetById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IUserPrivilegeService userPrivilegeService,
            IImportBookFacadeService service)
        {
            var employeeUrl = req.HttpContext.User.GetEmployeeUrl();
            var privs = await userPrivilegeService.GetUserPrivileges(employeeUrl);

            await res.Negotiate(await service.GetById(id, privs));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task Create(
            HttpRequest req,
            HttpResponse res,
            ImportBookResource resource,
            IUserPrivilegeService userPrivilegeService,
            IImportBookFacadeService service)
        {
            if (resource.CreatedById == null)
            {
                resource.CreatedById = req.HttpContext.User.GetEmployeeNumber();
            }

            var employeeUrl = req.HttpContext.User.GetEmployeeUrl();
            var privs = await userPrivilegeService.GetUserPrivileges(employeeUrl);

            await res.Negotiate(await service.Add(resource, privs));
        }

        private async Task Update(
            HttpResponse res,
            HttpRequest req,
            int id,
            ImportBookResource resource,
            IUserPrivilegeService userPrivilegeService,
            IImportBookFacadeService service)
        {
            var employeeUrl = req.HttpContext.User.GetEmployeeUrl();
            var privs = await userPrivilegeService.GetUserPrivileges(employeeUrl);

            await res.Negotiate(
                await service.Update(id, resource, privs));
        }

        private async Task Initialise(
            HttpRequest req,
            string rsnNumbers,
            string purchaseOrderNumbers,
            int? supplierId,
            HttpResponse res,
            IUserPrivilegeService userPrivilegeService,
            IImportBookFacadeService service)
        {
            var employeeUrl = req.HttpContext.User.GetEmployeeUrl();
            var privs = await userPrivilegeService.GetUserPrivileges(employeeUrl);

            await res.Negotiate(await service.InitialiseImportBook(rsnNumbers, purchaseOrderNumbers, supplierId, privs));
        }
    }
}
