namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
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
            app.MapGet("/requisitions/create", this.GetApp);
            app.MapGet("/requisitions/pending", this.GetApp);
            app.MapPost("/requisitions/cancel", this.Cancel);
            app.MapPost("/requisitions/book", this.Book);
            app.MapPost("/requisitions/authorise", this.Authorise);
            app.MapGet("/requisitions/stores-functions", this.GetFunctionCodes);
            app.MapGet("/requisitions/stores-functions/view", this.GetApp);
            app.MapGet("/requisitions/stores-functions/{code}", this.GetStoresFunction);
            app.MapGet("/requisitions/sundry-book-ins", this.GetSundryBookIns);
            app.MapPost("/requisitions/validate", this.Validate);
            app.MapGet("/requisitions/{reqNumber:int}", this.GetById);
            app.MapGet("/requisitions/application-state", this.GetRequisitionApplicationState);
            app.MapPost("/requisitions", this.Create);
            app.MapPost("/requisitions/{reqNumber}", this.Update);
            app.MapPost("/requisitions/print-qc-labels", this.PrintQcLabels);
            app.MapGet("/requisitions/{reqNumber:int}/preview-reversal", this.GetById);
        }

        private async Task Search(
            HttpRequest _,
            HttpResponse res,
            string comments,
            int? reqNumber,
            bool? includeCancelled,
            bool? pending,
            string documentName,
            int? documentNumber,
            bool? excludeReversals,
            bool? bookedOnly,
            string functionCode,
            IRequisitionFacadeService service)
        {
            if (!reqNumber.HasValue && string.IsNullOrWhiteSpace(comments) && string.IsNullOrWhiteSpace(documentName) && pending != true)
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
            }
            else
            {
                var requisitions = await service.FilterBy(
                    new RequisitionSearchResource
                    {
                        Comments = comments,
                        ReqNumber = reqNumber,
                        IncludeCancelled = includeCancelled.GetValueOrDefault(),
                        Pending = pending.GetValueOrDefault(),
                        DocumentName = documentName,
                        DocumentNumber = documentNumber,
                        ExcludeReversals = excludeReversals,
                        BookedOnly = bookedOnly,
                        FunctionCode = functionCode
                    });
                await res.Negotiate(requisitions);
            }
        }

        private async Task GetById(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.GetById(reqNumber, req.HttpContext.GetPrivileges()));
        }
        
        private async Task GetReversalPreview(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.GetReversalPreview(reqNumber));
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

        private async Task Book(
            HttpRequest req,
            HttpResponse res,
            BookRequisitionResource resource,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.BookRequisition(
                resource.ReqNumber,
                resource.LineNumber,
                req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault(),
                req.HttpContext.GetPrivileges()));
        }

        private async Task Authorise(
            HttpRequest req,
            HttpResponse res,
            AuthoriseRequisitionResource resource,
            IRequisitionFacadeService service)
        {
            await res.Negotiate(await service.AuthoriseRequisition(
                resource.ReqNumber,
                req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault(),
                req.HttpContext.GetPrivileges()));
        }

        private async Task GetRequisitionApplicationState(
            HttpRequest req,
            HttpResponse res,
            IRequisitionFacadeService service)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = service.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task Create(
            HttpResponse res, 
            HttpRequest req,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            resource.CreatedBy = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges(), resource.CreatedBy, false, true));
        }

        private async Task Validate(
            HttpResponse res,
            HttpRequest req,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            resource.CreatedBy = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(await service.Validate(resource));
        }


        private async Task Update(
            HttpResponse res,
            HttpRequest req,
            int reqNumber,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            var updatedBy = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(
                await service.Update(reqNumber, resource, req.HttpContext.GetPrivileges(), updatedBy, true));
        }

        private async Task GetFunctionCodes(
            HttpRequest req,
            HttpResponse res,
            IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource> service)
        {
            await res.Negotiate(await service.GetAll(req.HttpContext.GetPrivileges()));
        }

        private async Task GetStoresFunction(
            HttpRequest req,
            HttpResponse res,
            string code,
            IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource> service)
        {
            await res.Negotiate(await service.GetById(code, req.HttpContext.GetPrivileges()));
        }

        private async Task GetSundryBookIns(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            int orderLine,
            IAsyncQueryFacadeService<SundryBookInDetail, SundryBookInDetailResource, SundryBookInDetailResource> facadeService)
        {
            await res.Negotiate(
                await facadeService.FilterBy(
                    new SundryBookInDetailResource { OrderNumber = orderNumber, OrderLine = orderLine },
                    req.HttpContext.GetPrivileges()));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task PrintQcLabels(
            HttpResponse res, 
            HttpRequest req,
            IRequisitionLabelsFacadeService service,
            QcLabelPrintRequestResource resource)
        {
            resource.UserNumber = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(await service.PrintQcLables(resource));
        }
    }
}
