namespace Linn.Stores2.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Labels;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Reports;
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
            services.AddScoped<ServiceDbContext>().AddScoped<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddScoped<ITransactionManager, TransactionManager>()
                .AddScoped<IRepository<Country, string>, EntityFrameworkRepository<Country, string>>(
                    r => new EntityFrameworkRepository<Country, string>(r.GetService<ServiceDbContext>()?.Countries))
                .AddScoped<IRepository<Carrier, string>, CarrierRepository>()
                .AddScoped<IRepository<StockLocator, int>, StockLocatorRepository>()
                .AddScoped<IRequisitionRepository, RequisitionRepository>()
                .AddScoped<IRepository<StorageType, string>, EntityFrameworkRepository<StorageType, string>>(
                    r => new EntityFrameworkRepository<StorageType, string>(
                        r.GetService<ServiceDbContext>()?.StorageTypes))
                .AddScoped<IRepository<Address, string>, EntityFrameworkRepository<Address, string>>(
                    r => new EntityFrameworkRepository<Address, string>(
                        r.GetService<ServiceDbContext>()?.Addresses))
                .AddScoped<IRepository<StockPool, string>, StockPoolRepository>()
                .AddScoped<IRepository<PartStorageType, int>, PartStorageTypeRepository>()
                .AddScoped<IRepository<StoresBudget, int>, StoresBudgetRepository>()
                .AddScoped<IQueryRepository<StoragePlace>, EntityFrameworkQueryRepository<StoragePlace>>(
                    r => new EntityFrameworkQueryRepository<StoragePlace>(
                        r.GetService<ServiceDbContext>()?.StoragePlaces))
                .AddScoped<IRepository<AccountingCompany, string>, EntityFrameworkRepository<AccountingCompany, string>>(
                    r => new EntityFrameworkRepository<AccountingCompany, string>(
                        r.GetService<ServiceDbContext>()?.AccountingCompanies))
                .AddScoped<IRepository<StockState, string>, EntityFrameworkRepository<StockState, string>>(
                    r => new EntityFrameworkRepository<StockState, string>(
                        r.GetService<ServiceDbContext>()?.StockStates))
                .AddScoped<IRepository<StorageLocation, int>, StorageLocationRepository>()
                .AddScoped<IRepository<Employee, int>, EntityFrameworkRepository<Employee, int>>(
                    r => new EntityFrameworkRepository<Employee, int>(r.GetService<ServiceDbContext>()?.Employees))
                .AddScoped<IRepository<StorageSite, string>, StorageSiteRepository>()
                .AddScoped<IRepository<StoresFunction, string>, StoresFunctionRepository>()
                .AddScoped<IRepository<StoresPallet, int>, StoresPalletRepository>()
                .AddScoped<IRepository<Part, string>, EntityFrameworkRepository<Part, string>>(
                    r => new EntityFrameworkRepository<Part, string>(r.GetService<ServiceDbContext>()?.Parts))
                .AddScoped<IRepository<StoresTransactionDefinition, string>, EntityFrameworkRepository<StoresTransactionDefinition, string>>(
                    r => new EntityFrameworkRepository<StoresTransactionDefinition, string>(
                        r.GetService<ServiceDbContext>()?.StoresTransactionDefinition))
                .AddScoped<IRepository<Department, string>, EntityFrameworkRepository<Department, string>>(
                    r => new EntityFrameworkRepository<Department, string>(
                        r.GetService<ServiceDbContext>()?.Departments))
                .AddScoped<IRepository<Nominal, string>, EntityFrameworkRepository<Nominal, string>>(
                    r => new EntityFrameworkRepository<Nominal, string>(r.GetService<ServiceDbContext>()?.Nominals))
                .AddScoped<IRepository<GoodsInLogEntry, int>, GoodsInLogRepository>()
                .AddScoped<IRepository<StockTransaction, int>, StockTransactionRepository>()
                .AddScoped<IRepository<RequisitionHistory, int>, EntityFrameworkRepository<RequisitionHistory, int>>(
                    r => new EntityFrameworkRepository<RequisitionHistory, int>(
                        r.GetService<ServiceDbContext>()?.RequisitionHistory))
                .AddScoped<IRepository<StoresTransactionState, StoresTransactionStateKey>, EntityFrameworkRepository<StoresTransactionState, StoresTransactionStateKey>>(
                    r => new EntityFrameworkRepository<StoresTransactionState, StoresTransactionStateKey>(
                        r.GetService<ServiceDbContext>()?.StoresTransactionStates))
                .AddScoped<IRepository<PotentialMoveDetail, PotentialMoveDetailKey>, EntityFrameworkRepository<PotentialMoveDetail, PotentialMoveDetailKey>>(
                    r => new EntityFrameworkRepository<PotentialMoveDetail, PotentialMoveDetailKey>(
                        r.GetService<ServiceDbContext>()?.PotentialMoveDetails))
                .AddScoped<IQueryRepository<LabelType>, EntityFrameworkQueryRepository<LabelType>>(
                    r => new EntityFrameworkQueryRepository<LabelType>(r.GetService<ServiceDbContext>()?.LabelTypes))
                .AddScoped<IRepository<BookInOrderDetail, BookInOrderDetailKey>, EntityFrameworkRepository<BookInOrderDetail, BookInOrderDetailKey>>(
                    r => new EntityFrameworkRepository<BookInOrderDetail, BookInOrderDetailKey>(r.GetService<ServiceDbContext>()?.BookInOrderDetails))
                .AddTransient<IRepository<Cit, string>, EntityFrameworkRepository<Cit, string>>(r =>
                    new EntityFrameworkRepository<Cit, string>(r.GetService<ServiceDbContext>()?.Cits))
                    .AddScoped<IRepository<PcasStorageType, PcasStorageTypeKey>, PcasStorageTypeRepository>()
                .AddTransient<IRepository<PcasBoard, string>, EntityFrameworkRepository<PcasBoard, string>>(
                    r => new EntityFrameworkRepository<PcasBoard, string>(r.GetService<ServiceDbContext>()?.PcasBoards))
                .AddTransient<IRepository<NominalAccount, int>, EntityFrameworkRepository<NominalAccount, int>>(r =>
                    new EntityFrameworkRepository<NominalAccount, int>(r.GetService<ServiceDbContext>()?.NominalAccounts))
                .AddScoped<IRepository<WorkStation, string>, WorkStationRepository>()
                .AddScoped<IQueryRepository<SundryBookInDetail>, EntityFrameworkQueryRepository<SundryBookInDetail>>(
                    r => new EntityFrameworkQueryRepository<SundryBookInDetail>(r.GetService<ServiceDbContext>()?.SundryBookInDetails))
                .AddScoped<IQueryRepository<AuditLocation>, EntityFrameworkQueryRepository<AuditLocation>>(
                    r => new EntityFrameworkQueryRepository<AuditLocation>(r.GetService<ServiceDbContext>()?.AuditLocations))
                .AddScoped<IQueryRepository<LocationType>, EntityFrameworkQueryRepository<LocationType>>(r =>
                    new EntityFrameworkQueryRepository<LocationType>(r.GetService<ServiceDbContext>()?.LocationTypes))
                .AddScoped<IQueryRepository<TqmsData>, TqmsDataRepository>()
                .AddScoped<IQueryRepository<LabourHoursSummary>, EntityFrameworkQueryRepository<LabourHoursSummary>>(
                    r => new EntityFrameworkQueryRepository<LabourHoursSummary>(
                        r.GetService<ServiceDbContext>()?.LabourHourSummaries))
                .AddScoped<IQueryRepository<DailyEuDespatchReport>, EntityFrameworkQueryRepository<DailyEuDespatchReport>>(
                    r => new EntityFrameworkQueryRepository<DailyEuDespatchReport>(
                        r.GetService<ServiceDbContext>()?.DailyEuDespatchReport))
                .AddScoped<IQueryRepository<DailyEuRsnImportReport>, EntityFrameworkQueryRepository<DailyEuRsnImportReport>>(
                    r => new EntityFrameworkQueryRepository<DailyEuRsnImportReport>(
                        r.GetService<ServiceDbContext>()?.DailyEuRsnImportReport))
                .AddScoped<IRepository<Expbook, int>, ExpbookRepository>();
        }
    }
}
