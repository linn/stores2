namespace Linn.Stores2.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Persistence.LinnApps;
    using Linn.Stores2.Persistence.LinnApps.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services.AddScoped<ServiceDbContext>()
                .AddScoped<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddScoped<ITransactionManager, TransactionManager>()
                .AddScoped<IRepository<Country, string>, EntityFrameworkRepository<Country, string>>(
                    r => new EntityFrameworkRepository<Country, string>(r.GetService<ServiceDbContext>()?.Countries))
                .AddScoped<IRepository<Carrier, string>, CarrierRepository>()
                .AddScoped<IRepository<StockLocator, int>, StockLocatorRepository>()
                .AddScoped<IRepository<RequisitionHeader, int>, RequisitionRepository>();
        }
    }
}
