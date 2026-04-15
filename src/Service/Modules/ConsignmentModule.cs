namespace Linn.Stores2.Service.Modules
{
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ConsignmentModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/consignments/{consignmentNumber}/packing-list/view", this.GetPackingListHtml);
            app.MapGet("/stores2/consignments/{consignmentNumber}/packing-list/pdf", this.GetPackingListPdf);
            app.MapGet("/stores2/consignments/multiple-packing-lists/view", this.GetPackingListsHtml);
            app.MapGet("/stores2/consignments/multiple-packing-lists/pdf", this.GetPackingListsPdf);
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
    }
}
