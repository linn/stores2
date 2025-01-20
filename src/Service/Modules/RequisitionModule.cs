namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class RequisitionModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/requisitions", this.Search);
            app.MapGet("/requisitions/{reqNumber}", this.GetById);
            app.MapPost("/requisitions/cancel/{reqNumber}", this.Cancel);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string comments,
            int? reqNumber,
            bool? includeCancelled,
            IRequisitionFacadeService service)
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
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.GetById(reqNumber));
        }

        private async Task Cancel(
            HttpRequest req,
            HttpResponse res,
            CancelRequisitionResource resource,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.Cancel(
                                    resource.ReqNumber, 
                                    req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault(),
                                    resource.Reason,
                                    req.HttpContext.GetPrivileges()));
        }
    }
}
