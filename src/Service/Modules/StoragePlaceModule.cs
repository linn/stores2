namespace Linn.Stores2.Service.Modules
{
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Service.Extensions;
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
            app.MapPost("/stores2/storage-places/create-checked-audit-reqs", this.CreateAuditReqs);
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
            IStoragePlaceAuditReportFacadeService facadeService)
        {
            var result = await facadeService.GetStoragePlaceAuditReportAsPdf(locationList, locationRange);

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }

        private async Task CreateAuditReqs(
            HttpRequest req,
            HttpResponse res,
            StoragePlaceRequestResource resource,
            IStoragePlaceAuditReportFacadeService facadeService)
        {
            var result = await facadeService.CreateCheckedAuditReqs(
                resource.LocationList,
                resource.LocationRange,
                resource.EmployeeNumber,
                resource.DepartmentCode,
                req.HttpContext.GetPrivileges());
            await res.Negotiate(result);
        }
    }
}
