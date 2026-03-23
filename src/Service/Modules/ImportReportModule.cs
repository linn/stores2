namespace Linn.Stores2.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Linn.Common.Service;
    using Linn.Common.Service.Extensions;
    using Linn.Stores2.Facade.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ImportReportModule : IModule
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/stores2/import-books/{id:int}/instruction/view", this.ImportClearanceInstructionAsHtml);
            app.MapGet("/stores2/import-books/{id:int}/instruction/pdf", this.ImportClearanceInstructionAsPdf);
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

            res.ContentType = "application/pdf";
            await res.FromStream(result, res.ContentType, new System.Net.Mime.ContentDisposition("attachment"));
        }
    }
}
