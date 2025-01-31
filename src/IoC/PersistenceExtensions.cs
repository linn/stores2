namespace Linn.Stores2.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Persistence.LinnApps;
    using Linn.Stores2.Persistence.LinnApps.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class PersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services
                .AddScoped<ServiceDbContext>()
                .AddScoped<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddScoped<ITransactionManager, TransactionManager>()
                .AddScoped<IRepository<Country, string>, EntityFrameworkRepository<Country, string>>(
                    r => new EntityFrameworkRepository<Country, string>(r.GetService<ServiceDbContext>()?.Countries))
                .AddScoped<IRepository<Carrier, string>, CarrierRepository>()
                .AddScoped<IRepository<StockLocator, int>, StockLocatorRepository>()
                .AddScoped<IRepository<RequisitionHeader, int>, RequisitionRepository>()
                .AddScoped<IRepository<StorageType, string>, EntityFrameworkRepository<StorageType, string>>(
                    r => new EntityFrameworkRepository<StorageType, string>(r.GetService<ServiceDbContext>()?.StorageTypes))
                .AddScoped<IRepository<StockPool, string>, StockPoolRepository>()
                .AddScoped<IRepository<PartsStorageType, PartsStorageTypeKey>, PartsStorageTypeRepository>()
                .AddScoped<IRepository<StoresBudget, int>, StoresBudgetRepository>()
                .AddScoped<IQueryRepository<StoragePlace>, EntityFrameworkQueryRepository<StoragePlace>>(
                    r => new EntityFrameworkQueryRepository<StoragePlace>(r.GetService<ServiceDbContext>()?.StoragePlaces))
                .AddScoped<IRepository<AccountingCompany, string>, EntityFrameworkRepository<AccountingCompany, string>>(
                    r => new EntityFrameworkRepository<AccountingCompany, string>(r.GetService<ServiceDbContext>()?.AccountingCompanies))
                .AddScoped<IRepository<StorageLocation, int>, StorageLocationRepository>()
                .AddScoped<IRepository<Employee, int>, EntityFrameworkRepository<Employee, int>>(
                    r => new EntityFrameworkRepository<Employee, int>(r.GetService<ServiceDbContext>()?.Employees))
                .AddScoped<IRepository<StorageSite, string>, StorageSiteRepository>()
                .AddScoped<IRepository<StoresFunctionCode, string>, StoresFunctionCodeRepository>();
        }
    }
}
