namespace Linn.Stores2.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Resources;
    using Linn.Common.Service.Handlers;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.External;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Pcas;
    using Linn.Stores2.Resources.Requisitions;
    using Linn.Stores2.Resources.Stores;
    using Linn.Stores2.Service.ResultHandlers;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.AddSingleton<IHandler, JsonResultHandler<IEnumerable<CountryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, CsvResultHandler<ReportReturnResource>>()
                .AddSingleton<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddSingleton<IHandler, JsonResultHandler<CarrierResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<CarrierResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StoresBudgetResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StoresBudgetResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<RequisitionHeaderResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<RequisitionHeaderResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StorageTypeResource>>()
                .AddTransient<IHandler, RequisitionApplicationStateResultHandler>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageTypeResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<PartsStorageTypeResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<PartsStorageTypeResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<SundryBookInDetailResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<AuditLocationResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StockPoolResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StockPoolResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageSiteResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StockStateResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StockStateResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StorageLocationResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageLocationResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StoresFunctionResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StoresFunctionResource>>()
                .AddSingleton<IHandler, JsonResultHandler<WorkStationResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<WorkStationResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<PcasStorageTypeResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<PcasStorageTypeResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<PcasBoardResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<PcasBoardResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StoresPalletResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StoresPalletResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<LocationTypeResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<DepartmentResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<DepartmentResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<EmployeeResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<EmployeeResource>>()
                .AddTransient<IHandler, WorkStationsApplicationStateResultHandler>()
                .AddSingleton<IHandler, JsonResultHandler<StorageSiteResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageSiteResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<TotalResource>>();
        }
    }
}
