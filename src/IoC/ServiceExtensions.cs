namespace Linn.Stores2.IoC
{
    using System.Net.Http;
    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Domain.LinnApps.Services;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Proxy;
    using Linn.Common.Proxy.LinnApps.Services;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Reports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Proxy.HttpClients;
    using Linn.Stores2.Proxy.StoredProcedureClients;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Pcas;
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
                .AddScoped<IRequisitionManager, RequisitionManager>()
                .AddScoped<IRequisitionFactory, RequisitionFactory>()
                .AddScoped<IRequisitionStoredProcedures, RequisitionStoredProcedures>()
                .AddScoped<IStoragePlaceAuditPack, StoragePlaceAuditPack>()
                .AddTransient<IDatabaseSequenceService, DatabaseSequenceService>()
                .AddTransient<IDatabaseService, DatabaseService>()
                .AddTransient<IRestClient, RestClient>()
                .AddScoped<IDocumentProxy, DocumentProxy>()
                .AddTransient<IStockService, StockService>()
                .AddTransient<IStoresService, StoresService>()
                .AddTransient<ILabelPrinter, BartenderLabelPack>()
                .AddScoped<IGoodsInLogReportService, GoodsInLogReportService>()
                .AddScoped<ICreationStrategyResolver, RequisitionCreationStrategyResolver>()
                .AddScoped<LdreqCreationStrategy>()
                .AddScoped<AutomaticBookFromHeaderStrategy>()
                .AddScoped<LinesProvidedStrategy>()
                .AddScoped<LoanOutCreationStrategy>()
                .AddScoped<IStoresTransViewerReportService, StoresTransViewerReportService>()
                .AddScoped<ISalesProxy, SalesProxy>()
                .AddScoped<IBomVerificationProxy, BomVerificationProxy>()
                .AddScoped<SuReqCreationStrategy>()
                .AddTransient<IQcLabelPrinterService, QcLabelPrinterService>()
                .AddScoped<IDeliveryNoteService, DeliveryNoteService>()
                .AddScoped<IHtmlTemplateService<DeliveryNoteDocument>>(
                    x => new HtmlTemplateService<DeliveryNoteDocument>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}DeliveryNoteDocument.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<ISupplierProxy, SupplierProxy>();
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
                .AddScoped<IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource>, StorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource>, PartsStorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource>, StockPoolFacadeService>()
                .AddScoped<IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>, StorageSiteService>()
                .AddScoped<IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource>, StorageLocationService>()
                .AddScoped<IAsyncFacadeService<StockState, string, StockStateResource, StockStateResource, StockStateResource>, StockStateFacadeService>()
                .AddScoped<IAsyncQueryFacadeService<SundryBookInDetail, SundryBookInDetailResource, SundryBookInDetailResource>, SundryBookInDetailFacadeService>()
                .AddScoped<IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource>, StoresFunctionCodeService>()
                .AddScoped<IGoodsInLogReportFacadeService, GoodsInLogReportFacadeService>()
                .AddScoped<IStoresTransViewerReportFacadeService, StoresTransViewerReportFacadeService>()
                .AddScoped<IAsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource>, WorkstationFacadeService>()
                .AddScoped<IRequisitionLabelsFacadeService, RequisitionLabelsFacadeService>()
                .AddScoped<IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource>, PcasStorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<PcasBoard, string, PcasBoardResource, PcasBoardResource, PcasBoardResource>, PcasBoardService>()
                .AddScoped<IDeliveryNoteFacadeService, DeliveryNoteFacadeService>();
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            return services.AddScoped<IBuilder<Carrier>, CarrierResourceBuilder>()
                .AddScoped<IBuilder<Country>, CountryResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<RequisitionHeader>, RequisitionResourceBuilder>()
                .AddScoped<IBuilder<StoresFunction>, StoresFunctionResourceBuilder>()
                .AddScoped<IBuilder<NominalAccount>, NominalAccountResourceBuilder>()
                .AddScoped<IBuilder<StoresBudget>, StoresBudgetResourceBuilder>()
                .AddScoped<IBuilder<StockState>, StockStateResourceBuilder>()
                .AddScoped<IBuilder<StorageSite>, StorageSiteResourceBuilder>()
                .AddScoped<IBuilder<StorageType>, StorageTypeResourceBuilder>()
                .AddScoped<IBuilder<PartsStorageType>, PartsStorageTypeResourceBuilder>()
                .AddScoped<IBuilder<SundryBookInDetail>, SundryBookInDetailResourceBuilder>()
                .AddScoped<IBuilder<StorageLocation>, StorageLocationResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<StockPool>, StockPoolResourceBuilder>()
                .AddTransient<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<Workstation>, WorkstationResourceBuilder>()
                .AddScoped<IBuilder<WorkstationElement>, WorkstationElementsResourceBuilder>()
                .AddScoped<IBuilder<PcasStorageType>, PcasStorageTypeResourceBuilder>()
                .AddScoped<IBuilder<PcasBoard>, PcasBoardResourceBuilder>();
        }
    }
}
