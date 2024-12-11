namespace Linn.Stores2.IoC
{
    using Linn.Common.Service.Core.Handlers;
    using Linn.Stores2.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHandler, JsonResultHandler<CountryResource>>() // singleton? or scoped?
                .AddSingleton<IHandler, JsonResultHandler<ProcessResultResource>>();
        }
    }
}
