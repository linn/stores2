﻿namespace Linn.Stores2.IoC
{
    using System.Net.Http;

    using Linn.Common.Configuration;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Resources;

    using Microsoft.Extensions.DependencyInjection;

    using RazorEngineCore;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IReportingHelper, ReportingHelper>()
                .AddSingleton<ITemplateEngine, RazorTemplateEngine>()
                .AddTransient<IPdfService>(
                    x => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()))
                .AddScoped<IStoragePlaceAuditReportService, StoragePlaceAuditReportService>()
                .AddScoped<IHtmlTemplateService<StoragePlaceAuditReport>>(
                    x => new HtmlTemplateService<StoragePlaceAuditReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}StoragePlaceAudit.cshtml",
                        x.GetService<ITemplateEngine>()));
        }

        public static IServiceCollection AddFacadeServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRazorEngine, RazorEngine>()
                .AddScoped<IStoragePlaceAuditReportFacadeService, StoragePlaceAuditReportFacadeService>()
                .AddScoped<IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource>, CarrierService>()
                .AddScoped<IAsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource>, CountryService>()
                .AddScoped<IAsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionHeaderResource>, RequisitionFacadeService>();
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            return services.AddScoped<IBuilder<Carrier>, CarrierResourceBuilder>()
                .AddScoped<IBuilder<Country>, CountryResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                // I think this can be a singleton
                .AddSingleton<IBuilder<RequisitionHeader>, RequisitionResourceBuilder>();
        }
    }
}
