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
    using Linn.Stores2.Facade.ResourceBuilders;
    using Linn.Stores2.Facade.Services;
    using Linn.Stores2.Proxy.HttpClients;
    using Linn.Stores2.Proxy.StoredProcedureClients;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Pcas;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Resources.Stores;

    using Microsoft.Extensions.DependencyInjection;
    using RazorEngineCore;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IRestClient, RestClient>();

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
                .AddTransient<IDatabaseSequenceService, DatabaseSequenceService>()
                .AddTransient<IStorageTypeService, StorageTypesService>()
                .AddTransient<IDatabaseService, DatabaseService>()
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
                .AddScoped<IRequisitionReportService, RequisitionReportService>()
                .AddScoped<ISalesProxy, SalesProxy>()
                .AddScoped<IBomVerificationProxy, BomVerificationProxy>()
                .AddScoped<SuReqCreationStrategy>()
                .AddTransient<IQcLabelPrinterService, QcLabelPrinterService>()
                .AddScoped<IDeliveryNoteService, DeliveryNoteService>()
                .AddScoped<IHtmlTemplateService<DeliveryNoteDocument>>(
                    x => new HtmlTemplateService<DeliveryNoteDocument>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}DeliveryNoteDocument.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<IHtmlTemplateService<RequisitionHeader>>(
                    x => new HtmlTemplateService<RequisitionHeader>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}Requisition.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<IHtmlTemplateService<RequisitionCostReport>>(
                    x => new HtmlTemplateService<RequisitionCostReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}RequisitionCost.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<ISupplierProxy, SupplierProxy>()
                .AddScoped<ISerialNumberService, SerialNumberService>()
                .AddScoped<IStockReportService, StockReportService>()
                .AddTransient<ICalcLabourHoursProxy, CalcLabourTimesProxy>()
                .AddScoped<IHtmlTemplateService<LabourHoursInStockReport>>(
                    x => new HtmlTemplateService<LabourHoursInStockReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}LabourHoursInStock.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<IHtmlTemplateService<LabourHoursSummaryReport>>(
                    x => new HtmlTemplateService<LabourHoursSummaryReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}LabourHoursSummary.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddScoped<IHtmlTemplateService<LabourHoursInLoansReport>>(
                    x => new HtmlTemplateService<LabourHoursInLoansReport>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}LabourHoursInLoans.cshtml",
                        x.GetService<ITemplateEngine>()));
        }

        public static IServiceCollection AddFacadeServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRazorEngine, RazorEngine>()
                .AddScoped<IStoragePlaceAuditReportFacadeService, StoragePlaceAuditReportFacadeService>()
                .AddScoped<IAsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource>, CarrierService>()
                .AddScoped<IAsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetSearchResource>, StoresBudgetFacadeService>()
                .AddScoped<IAsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource>, CountryService>()
                .AddScoped<IRequisitionFacadeService, RequisitionFacadeService>()
                .AddScoped<IRequisitionReportFacadeService, RequisitionReportFacadeService>()
                .AddScoped<IAsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource>, StorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource>, PartsStorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource>, StockPoolFacadeService>()
                .AddScoped<IAsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>, StorageSiteFacadeService>()
                .AddScoped<IAsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationSearchResource>, StorageLocationService>()
                .AddScoped<IAsyncFacadeService<StockState, string, StockStateResource, StockStateResource, StockStateResource>, StockStateFacadeService>()
                .AddScoped<IAsyncQueryFacadeService<SundryBookInDetail, SundryBookInDetailResource, SundryBookInDetailResource>, SundryBookInDetailFacadeService>()
                .AddScoped<IAsyncQueryFacadeService<AuditLocation, AuditLocationResource, AuditLocationResource>, AuditLocationFacadeService>()
                .AddScoped<IAsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource>, StoresFunctionCodeService>()
                .AddScoped<IGoodsInLogReportFacadeService, GoodsInLogReportFacadeService>()
                .AddScoped<IStoresTransViewerReportFacadeService, StoresTransViewerReportFacadeService>()
                .AddScoped<IAsyncFacadeService<WorkStation, string, WorkStationResource, WorkStationResource, WorkStationSearchResource>, WorkStationFacadeService>()
                .AddScoped<IRequisitionLabelsFacadeService, RequisitionLabelsFacadeService>()
                .AddScoped<IAsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource>, PcasStorageTypeFacadeService>()
                .AddScoped<IAsyncFacadeService<PcasBoard, string, PcasBoardResource, PcasBoardResource, PcasBoardResource>, PcasBoardService>()
                .AddScoped<IAsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource>, StoresPalletFacadeService>()
                .AddScoped<IDeliveryNoteFacadeService, DeliveryNoteFacadeService>()
                .AddScoped<IStockReportFacadeService, StockReportFacadeService>();
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
                .AddScoped<IBuilder<AuditLocation>, AuditLocationResourceBuilder>()
                .AddScoped<IBuilder<StorageLocation>, StorageLocationResourceBuilder>()
                .AddScoped<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<StockPool>, StockPoolResourceBuilder>()
                .AddTransient<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddScoped<IBuilder<WorkStation>, WorkStationResourceBuilder>()
                .AddScoped<IBuilder<WorkStationElement>, WorkStationElementsResourceBuilder>()
                .AddScoped<IBuilder<PcasStorageType>, PcasStorageTypeResourceBuilder>()
                .AddScoped<IBuilder<PcasBoard>, PcasBoardResourceBuilder>()
                .AddScoped<IBuilder<StoresPallet>, StoresPalletResourceBuilder>()
                .AddScoped<IBuilder<LocationType>, LocationTypeResourceBuilder>()
                .AddScoped<IBuilder<Employee>, EmployeeResourceBuilder>()
                .AddScoped<IBuilder<Department>, DepartmentResourceBuilder>();
        }
    }
}
