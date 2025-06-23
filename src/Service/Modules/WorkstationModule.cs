namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Stores;
    using Linn.Stores2.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class WorkstationModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/work-stations", this.Search);
            app.MapPost("/stores2/work-stations", this.Create);
            app.MapGet("/stores2/work-stations/{*code}", this.GetById);
            app.MapPut("/stores2/work-stations/{*code}", this.Update);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string workstationCode,
            string citCode,
            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> service)
        {
            var workstations = await service.FilterBy(
                                   new WorkstationSearchResource
                                       {
                                           WorkStationCode = workstationCode,
                                           CitCode = citCode
                                       },
                                   req.HttpContext.GetPrivileges());
            await res.Negotiate(workstations);
        }

        private async Task GetById(
            HttpRequest req,
            HttpResponse res,
            string code,
            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> service)
        {
            await res.Negotiate(await service.GetById(code, req.HttpContext.GetPrivileges()));
        }

        private async Task Create(
            HttpRequest req,
            HttpResponse res,
            WorkstationResource resource,
            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> service)
        {
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges()));
        }

        private async Task Update(
            HttpRequest req,
            HttpResponse res,
            string code,
            WorkstationResource resource,
            IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource> service)
        {
            await res.Negotiate(await service.Update(code, resource, req.HttpContext.GetPrivileges()));
        }
    }
}
