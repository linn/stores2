namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class RequisitionModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/requisitions", this.Search);
            app.MapGet("/requisitions/{reqNumber}", this.GetById);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string comments,
            int? reqNumber,
            bool? includeCancelled,
            IAsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionSearchResource> service)
        {
            await res.Negotiate(await service.FilterBy(
                                    new RequisitionSearchResource
                                        {
                                            Comments = comments,
                                            ReqNumber = reqNumber,
                                            IncludeCancelled = includeCancelled.GetValueOrDefault()
                                        }, 
                                    null));
        }

        private async Task GetById(
            HttpRequest _,
            HttpResponse res,
            int reqNumber,
            IAsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionSearchResource> service)
        {
            await res.Negotiate(await service.GetById(reqNumber));
        }
    }
}
