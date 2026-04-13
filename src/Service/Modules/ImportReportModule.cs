namespace Linn.Stores2.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ImportReportModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/import-books/clearance-instruction", this.GetApp);
            app.MapGet("/stores2/import-books/clearance-instruction/view", this.ClearanceInstructionAsHtml);
            app.MapGet("/stores2/import-books/clearance-instruction/pdf", this.ClearanceInstructionAsPdf);
            app.MapGet("/stores2/import-books/{id:int}/instruction/view", this.ImportClearanceInstructionAsHtml);
            app.MapGet("/stores2/import-books/{id:int}/instruction/pdf", this.ImportClearanceInstructionAsPdf);
            app.MapGet("/stores2/import-books/report", this.ImportReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
        }

        private async Task ClearanceInstructionAsHtml(
            HttpResponse res,
            string impbooks,
            string toEmailAddress,
            IImportReportFacadeService facadeService)
        {
            var ids = Array.ConvertAll(impbooks.Split(','), int.Parse);
            var result = await facadeService.GetClearanceInstructionAsHtml(ids, toEmailAddress);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task ClearanceInstructionAsPdf(
            HttpResponse res,
            string impbooks,
            string toEmailAddress,
            IImportReportFacadeService facadeService)
        {
            var ids = Array.ConvertAll(impbooks.Split(','), int.Parse);
            var result = await facadeService.GetClearanceInstructionAsPdf(ids, toEmailAddress);
            await res.Negotiate(result);
        }

        private async Task ImportClearanceInstructionAsHtml(
            HttpResponse res,
            int id,
            string toEmailAddress,
            IImportReportFacadeService facadeService)
        {
            var result = await facadeService.GetClearanceInstructionAsHtml(new List<int> { id }, toEmailAddress);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task ImportClearanceInstructionAsPdf(
            HttpResponse res,
            int id,
            string toEmailAddress,
            IImportReportFacadeService facadeService)
        {
            var result = await facadeService.GetClearanceInstructionAsPdf(new List<int> { id }, toEmailAddress);
            await res.Negotiate(result);
        }

        private async Task ImportReport(
            HttpResponse res,
            string transportBillNumber,
            string customsEntryCode,
            int? rsnNumber,
            int? poNumber,
            string dateField,
            string fromDate,
            string toDate,
            IImportReportFacadeService facadeService)
        {
            var searchResource = new ImportBookSearchResource
            {
                TransportBillNumber = transportBillNumber,
                CustomsEntryCode = customsEntryCode,
                RsnNumber = rsnNumber,
                PONumber = poNumber,
                DateField = dateField,
                FromDate = fromDate,
                ToDate = toDate
            };

            await res.Negotiate(await facadeService.GetImportBookReport(searchResource));
        }
    }
}
