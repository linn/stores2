namespace Linn.Stores2.IoC
{
    using Linn.Common.Rendering;
    using Linn.Stores2.Facade.Services;

    using Microsoft.Extensions.DependencyInjection;

    using RazorEngineCore;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddFacade(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRazorEngine, RazorEngine>()
                .AddSingleton<ITemplateEngine, RazorTemplateEngine>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<ICarrierService, CarrierService>();
        }
    }
}
