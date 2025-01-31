namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Linq;
    using Linn.Stores2.Domain.LinnApps.External;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StorageLocationService : AsyncFacadeService<StorageLocation, int, StorageLocationResource, StorageLocationResource, StorageLocationResource>
    {
        private readonly IDatabaseSequenceService databaseSequenceService;
        private readonly IRepository<AccountingCompany, string> accountingCompanyRepository;
        private readonly IRepository<StorageSite, string> storageSiteRepository;
        private readonly IRepository<StockPool, string> stockPoolRepository;
        private readonly IRepository<StorageType, string> storageTypeRepository;

        public StorageLocationService(IRepository<StorageLocation, int> repository,
            ITransactionManager transactionManager, IBuilder<StorageLocation> resourceBuilder,
            IDatabaseSequenceService databaseSequenceService,
            IRepository<AccountingCompany, string> accountingCompanyRepository,
            IRepository<StorageSite, string> storageSiteRepository, IRepository<StockPool, string> stockPoolRepository,
            IRepository<StorageType, string> storageTypeRepository) : base(repository, transactionManager,
            resourceBuilder)
        {
            this.databaseSequenceService = databaseSequenceService;
            this.storageSiteRepository = storageSiteRepository;
            this.accountingCompanyRepository = accountingCompanyRepository;
            this.stockPoolRepository = stockPoolRepository;
            this.storageTypeRepository = storageTypeRepository;
        }

        protected override Expression<Func<StorageLocation, bool>> SearchExpression(string searchTerm)
        {
            return l => l.DateInvalid == null && l.LocationCode.Contains(searchTerm.ToUpper());
        }

        protected override Task SaveToLogTable(string actionType, int userNumber, StorageLocation entity, StorageLocationResource resource,
            StorageLocationResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(StorageLocation entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override async Task<StorageLocation> CreateFromResourceAsync(
            StorageLocationResource resource,
            IEnumerable<string> privileges = null)
        {
            var locationId = this.databaseSequenceService.NextStorageLocationId();

            var company = await this.accountingCompanyRepository.FindByIdAsync(resource.AccountingCompany);
            var site = await this.storageSiteRepository.FindByIdAsync(resource.SiteCode);
            var area = site == null
                ? null
                : site.StorageAreas?.FirstOrDefault(a => a.StorageAreaCode == resource.StorageAreaCode);

            var stockPool = await this.GetDefaultStockPool(resource.DefaultStockPool);

            var storageType = await this.GetStorageType(resource.StorageType);

            return new StorageLocation(locationId, resource.LocationCode, resource.Description, site, area, company,
                resource.AccessibleFlag, resource.StoresKittableFlag, resource.MixStatesFlag, resource.StockState, resource.TypeOfStock, 
                stockPool, storageType)
            {
                DefaultStockPool = resource.DefaultStockPool,
                StoresKittingPriority = resource.StoresKittingPriority,
                SpecProcFlag = resource.SpecProcFlag,
                LocationType = resource.LocationType,
                StorageTypeCode = resource.StorageType,
                SalesAccountId = resource.SalesAccountId,
                OutletNumber = resource.OutletNumber
            };
        }

        protected override async Task UpdateFromResourceAsync(
            StorageLocation entity,
            StorageLocationResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var company = await this.accountingCompanyRepository.FindByIdAsync(updateResource.AccountingCompany);

            var stockPool = await this.GetDefaultStockPool(updateResource.DefaultStockPool);

            var storageType = await this.GetStorageType(updateResource.StorageType);

            var dateInvalid = string.IsNullOrEmpty(updateResource.DateInvalid)
                ? (DateTime?)null
                : DateTime.Parse(updateResource.DateInvalid);

            entity.Update(updateResource.Description, company, updateResource.AccessibleFlag,
                updateResource.StoresKittableFlag, updateResource.MixStatesFlag, updateResource.StockState,
                updateResource.TypeOfStock, stockPool, storageType, dateInvalid);

            entity.AuditFrequencyWeeks = updateResource.AuditFrequencyWeeks;
            entity.StoresKittingPriority = updateResource.StoresKittingPriority;
        }

        protected override Expression<Func<StorageLocation, bool>> FilterExpression(StorageLocationResource searchResource)
        {
            Expression<Func<StorageLocation, bool>> expression = loc => loc.DateInvalid == null;

            if (!string.IsNullOrEmpty(searchResource.SiteCode))
            {
                expression = this.CombineExpression(expression, loc => loc.SiteCode == searchResource.SiteCode);
            }

            if (!string.IsNullOrEmpty(searchResource.StorageAreaCode))
            {
                expression = this.CombineExpression(expression, loc => loc.StorageAreaCode == searchResource.StorageAreaCode);
            }

            if (!string.IsNullOrEmpty(searchResource.LocationCode))
            {
                expression = this.CombineExpression(expression, loc => loc.LocationCode.Contains(searchResource.LocationCode.ToUpper()));
            }

            return expression;
        }

        protected override Expression<Func<StorageLocation, bool>> FindExpression(StorageLocationResource searchResource)
        {
            throw new NotImplementedException();
        }

        private Expression<Func<StorageLocation, bool>> CombineExpression(
            Expression<Func<StorageLocation, bool>> first,
            Expression<Func<StorageLocation, bool>> second)
        {
            // a bit ChatGPTy but other combine expression examples were worse
            var parameter = Expression.Parameter(typeof(StorageLocation), "x");

            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<StorageLocation, bool>>(body, parameter);
        }

        private async Task<StockPool> GetDefaultStockPool(string stockPoolCode)
        {
            if (string.IsNullOrEmpty(stockPoolCode))
            {
                return null;
            }
            var stockPool = await this.stockPoolRepository.FindByIdAsync(stockPoolCode);

            if (stockPool == null)
            {
                throw new StorageLocationException($"Cannot find stock pool {stockPoolCode}");
            }

            return stockPool;
        }

        private async Task<StorageType> GetStorageType(string storageTypeCode)
        {
            if (string.IsNullOrEmpty(storageTypeCode))
            {
                return null;
            }
            var storageType = await this.storageTypeRepository.FindByIdAsync(storageTypeCode);

            if (storageType == null)
            {
                throw new StorageLocationException($"Cannot find storage type {storageTypeCode}");
            }

            return storageType;
        }
    }
}
