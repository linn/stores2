namespace Linn.Stores2.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Resources;
    using Linn.Common.Service.Core.Handlers;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;
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
                .AddSingleton<IHandler, JsonResultHandler<StockPoolResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StockPoolResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageSiteResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StockStateResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StockStateResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StorageLocationResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StorageLocationResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<StoresFunctionResource>>>()
                .AddSingleton<IHandler, JsonResultHandler<StoresFunctionResource>>()
                .AddSingleton<IHandler, JsonResultHandler<WorkstationResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<WorkstationResource>>>();
        }
    }
}
