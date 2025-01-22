namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Extensions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class StoragePlaceModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/reports/storage-place-audit/report", this.StoragePlaceAuditReport);
            app.MapGet("/stores2/reports/storage-place-audit", this.GetApp);
            app.MapGet("/stores2/reports/storage-place-audit/pdf", this.StoragePlaceAuditReportAsPdf);
            
            //app.MapGet("/stores2/storage-places", this.SearchStorageLocations);
        }

        private async Task StoragePlaceAuditReport(
            HttpRequest _, 
            HttpResponse res,
            string[] locationList,
            string locationRange,
            IStoragePlaceAuditReportFacadeService facadeService)
        {
            await res.Negotiate(facadeService.GetStoragePlaceAuditReport(locationList, locationRange));
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task StoragePlaceAuditReportAsPdf(
            HttpRequest req,
            HttpResponse res,
            string[] locationList,
            string locationRange,
            IStoragePlaceAuditReportFacadeService documentsFacadeService)
        {
            var result = await documentsFacadeService.GetStoragePlaceAuditReportAsPdf(locationList, locationRange);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }

        // private async Task SearchStorageLocations(
        //     HttpResponse res,
        //     string searchTerm,
        //     IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource> service)
        // {
        //     await res.Negotiate(service.Search(searchTerm));
        // }
    }
}
