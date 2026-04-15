namespace Linn.Stores2.Service.Modules
{
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources.Consignments;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Service.Extensions;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ConsignmentModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/consignments/{consignmentNumber}", this.GetConsignment);
            app.MapGet("/stores2/consignments/{consignmentNumber}/packing-list/view", this.GetPackingListHtml);
            app.MapGet("/stores2/consignments/{consignmentNumber}/packing-list/pdf", this.GetPackingListPdf);
            app.MapGet("/stores2/consignments/packing-lists", this.GetApp);
            app.MapGet("/stores2/consignments/multiple-packing-lists/view", this.GetPackingListsHtml);
            app.MapGet("/stores2/consignments/multiple-packing-lists/pdf", this.GetPackingListsPdf);
        }

        private async Task GetConsignment(
            HttpRequest req,
            HttpResponse res,
            int consignmentNumber,
            IAsyncFacadeService<Consignment, int, ConsignmentResource, ConsignmentResource, ConsignmentSearchResource> facadeService,
            IUserPrivilegeService userPrivilegeService)
        {
            var user = req.HttpContext.User.GetEmployeeUrl();
            var privileges = await userPrivilegeService.GetUserPrivileges(user);

            await res.Negotiate(await facadeService.GetById(consignmentNumber, privileges));
        }

        private async Task GetPackingListHtml(
            HttpResponse res,
            int consignmentNumber,
            IPackingListFacadeService facadeService)
        {
            var result = await facadeService.GetPackingListAsHtml(consignmentNumber);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task GetPackingListPdf(
            HttpResponse res,
            int consignmentNumber,
            IPackingListFacadeService facadeService)
        {
            var result = await facadeService.GetPackingListAsPdf(consignmentNumber);

            await res.Negotiate(result);
        }

        private async Task GetPackingListsHtml(
            HttpResponse res,
            int[] consignmentNumber,
            IPackingListFacadeService facadeService)
        {
            var result = await facadeService.GetPackingListsAsHtml(consignmentNumber);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task GetPackingListsPdf(
            HttpResponse res,
            int[] consignmentNumber,
            IPackingListFacadeService facadeService)
        {
            var result = await facadeService.GetPackingListsAsPdf(consignmentNumber);

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }
    }
}
