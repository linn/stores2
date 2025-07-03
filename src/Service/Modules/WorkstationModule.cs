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
            app.MapGet("/stores2/work-stations/application-state", this.GetWorkstationApplicationState);
            app.MapGet("/stores2/work-stations", this.Search);
            app.MapPost("/stores2/work-stations", this.Create);
            app.MapGet("/stores2/work-stations/{*code}", this.GetById);
            app.MapPut("/stores2/work-stations/{*code}", this.Update);
        }

        private async Task GetWorkstationApplicationState(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> service)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = service.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string workStationCode,
            string citCode,
            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> service)
        {
            var workStations = await service.FilterBy(
                                   new WorkStationSearchResource
                                       {
                                           WorkStationCode = workStationCode,
                                           CitCode = citCode
                                       },
                                   req.HttpContext.GetPrivileges());
            await res.Negotiate(workStations);
        }

        private async Task GetById(
            HttpRequest req,
            HttpResponse res,
            string code,
            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> service)
        {
            await res.Negotiate(await service.GetById(code, req.HttpContext.GetPrivileges()));
        }

        private async Task Create(
            HttpRequest req,
            HttpResponse res,
            WorkStationResource resource,
            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> service)
        {
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges()));
        }

        private async Task Update(
            HttpRequest req,
            HttpResponse res,
            string code,
            WorkStationResource resource,
            IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource> service)
        {
            await res.Negotiate(await service.Update(code, resource, req.HttpContext.GetPrivileges()));
        }
    }
}
