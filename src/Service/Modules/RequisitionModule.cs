﻿namespace Linn.Stores2.Service.Modules
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
            app.MapGet("/requisitions/{reqNumber}", this.GetById);
            app.MapPost("/requisitions/cancel", this.Cancel);
            app.MapPost("/requisitions/book", this.Book);
            app.MapPost("/requisitions/authorise", this.Authorise);
            app.MapPost("/requisitions", this.Create);
            app.MapPut("/requisitions/{reqNumber}", this.Update);
            app.MapGet("/requisitions/function-codes", this.GetFunctionCodes);
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
                        DocumentNumber = documentNumber
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

        private async Task Create(
            HttpResponse res, 
            HttpRequest req,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            resource.CreatedBy = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(await service.Add(resource, req.HttpContext.GetPrivileges(), resource.CreatedBy, false, true));
        }

        private async Task Update(
            HttpResponse res,
            HttpRequest req,
            int reqNumber,
            RequisitionHeaderResource resource,
            IRequisitionFacadeService service)
        {
            var updatedBy = req.HttpContext.User.GetEmployeeNumber().GetValueOrDefault();
            await res.Negotiate(await service.Update(reqNumber, resource, req.HttpContext.GetPrivileges(), updatedBy));
        }

        private async Task GetFunctionCodes(
            HttpRequest _,
            HttpResponse res,
            IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource> service)
        {
            await res.Negotiate(await service.GetAll());
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
