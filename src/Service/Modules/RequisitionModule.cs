namespace Linn.Stores2.Service.Modules
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class RequisitionModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/requisitions", this.Search);
            app.MapGet("/stores2/requisitions/{reqNumber}", this.GetById);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            RequisitionHeaderResource searchResource,
            IAsyncFacadeService<RequisitionHeaderResource, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionHeaderResource> service)
        {
            await res.Negotiate(await service.FilterBy(searchResource, null));
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int reqNumber,
            IAsyncFacadeService<RequisitionHeaderResource, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionHeaderResource> service)
        {
            await res.Negotiate(await service.GetById(reqNumber));
        }
    }
}
