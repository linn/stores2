namespace Linn.Stores2.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
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
            app.MapPost("/stores2/import-books/comparer/view", this.ImportBookComparerReport);
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

        private async Task ImportBookComparerReport(
            HttpRequest req,
            HttpResponse res,
            string fromDate,
            string toDate,
            IImportBookUploadService service)
        {
            using var reader = new StreamReader(req.Body);
            var csvContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(csvContent))
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await res.WriteAsJsonAsync(new { error = "No CSV content provided" });
                return;
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvContent);
            using var csvStream = new MemoryStream(csvBytes);

            var result = await service.GetImportBookComparerWithCsvReport(
                DateTime.Parse(fromDate),
                DateTime.Parse(toDate),
                csvStream);

            await res.Negotiate(result);
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
    }
}
