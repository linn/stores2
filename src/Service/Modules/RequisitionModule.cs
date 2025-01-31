namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Service.Extensions;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class RequisitionModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/requisitions", this.Search);
            app.MapGet("/requisitions/{reqNumber}", this.GetById);
            app.MapPost("/requisitions/cancel", this.Cancel);
            app.MapPost("/requisitions", this.Create);
            app.MapGet("/requisitions/function-codes", this.GetFunctionCodes);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string comments,
            int? reqNumber,
            bool? includeCancelled,
            IRequisitionFacadeService service)
        {
            if (!reqNumber.HasValue && string.IsNullOrWhiteSpace(comments))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                await res.Negotiate(await service.FilterBy(
                                        new RequisitionSearchResource
                                            {
                                                Comments = comments,
                                                ReqNumber = reqNumber,
                                                IncludeCancelled = includeCancelled.GetValueOrDefault()
                                            }));
            }
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
            if (resource.LineNumber.HasValue)
            {
                await res.Negotiate(await service.CancelLine(
                                        resource.ReqNumber,
                                        resource.LineNumber.Value,
                                        req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault(),
                                        resource.Reason,
                                        req.HttpContext.GetPrivileges()));
            }
            else
            {
                await res.Negotiate(await service.CancelHeader(
                                        resource.ReqNumber,
                                        req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault(),
                                        resource.Reason,
                                        req.HttpContext.GetPrivileges()));
            }
        }

        private async Task Create(HttpResponse res, 
            HttpRequest req,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(service.Add(resource, req.HttpContext.GetPrivileges()));
        }
        
        private async Task GetFunctionCodes(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<StoresFunctionCode, string, FunctionCodeResource, FunctionCodeResource, FunctionCodeResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }
    }
}
