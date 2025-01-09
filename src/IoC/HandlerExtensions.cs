﻿namespace Linn.Stores2.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Service.Core.Handlers;
    using Linn.Stores2.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<CountryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, CsvResultHandler<ReportReturnResource>>()
                .AddSingleton<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddSingleton<IHandler, JsonResultHandler<CarrierResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<CarrierResource>>>();
        }
    }
}
