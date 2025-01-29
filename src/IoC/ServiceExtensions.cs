namespace Linn.Stores2.IoC
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Proxy;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.GoodsIn;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Resources.Stores;

    using Microsoft.Extensions.DependencyInjection;

    using RazorEngineCore;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IReportingHelper, ReportingHelper>()
                .AddSingleton<ITemplateEngine, RazorTemplateEngine>()
                .AddSingleton<IAuthorisationService, AuthorisationService>()
                .AddScoped<IPdfService>(
                    _ => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()))
                .AddScoped<IStoragePlaceAuditReportService, StoragePlaceAuditReportService>()
                .AddScoped<IHtmlTemplateService<StoragePlaceAuditReport>>(
                    x => new HtmlTemplateService<StoragePlaceAuditReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}StoragePlaceAudit.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<IRequisitionService, RequisitionService>()
                .AddScoped<IRequisitionStoredProcedures, RequisitionStoredProcedures>()
                .AddScoped<IStoragePlaceAuditPack, StoragePlaceAuditPack>();
        }

        public static IServiceCollection AddFacadeServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRazorEngine, RazorEngine>()
                .AddScoped<IStoragePlaceAuditReportFacadeService, StoragePlaceAuditReportFacadeService>()
                .AddScoped<IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource>, CarrierService>()
                .AddScoped<IAsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetResource>, StoresBudgetFacadeService>()
                .AddScoped<IAsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource>, CountryService>()
                .AddScoped<IRequisitionFacadeService, RequisitionFacadeService>()
                .AddScoped<IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource>, StockPoolFacadeService>()
                .AddScoped<IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>, StorageSiteService>()
                .AddScoped<IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource>, StorageLocationService>()
                .AddScoped<IAsyncFacadeService<GoodsInLogEntry, int, GoodsInLogEntryResource, GoodsInLogEntryResource, GoodsInLogEntrySearchResource>, GoodsInLogFacadeService>();
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            return services.AddScoped<IBuilder<Carrier>, CarrierResourceBuilder>()
                .AddScoped<IBuilder<Country>, CountryResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<RequisitionHeader>, RequisitionResourceBuilder>()
                .AddScoped<IBuilder<NominalAccount>, NominalAccountResourceBuilder>()
                .AddScoped<IBuilder<StoresBudget>, StoresBudgetResourceBuilder>()
                .AddScoped<IBuilder<StorageSite>, StorageSiteResourceBuilder>()
                .AddScoped<IBuilder<StorageLocation>, StorageLocationResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddTransient<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<GoodsInLogEntry>, GoodsInLogEntryResourceBuilder>();
        }
    }
}
