namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Amazon.SimpleEmail.Model;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StoresPalletFacadeService : AsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource>
    {
        private readonly IRepository<StockPool, string> stockPoolRepository;

        private readonly IRepository<LocationType, string> locationTypeRepository;

        private readonly IRepository<StorageLocation, int> storageLocationTypeRepository;

        private readonly IRepository<StoresPallet, int> palletRepository;
        public StoresPalletFacadeService(
            IRepository<StoresPallet, int> repository,
            ITransactionManager transactionManager,
            IBuilder<StoresPallet> resourceBuilder,
            IRepository<StockPool, string> stockPoolRepository,
            IRepository<LocationType, string> locationTypeRepository,
            IRepository<StorageLocation, int> storageLocationTypeRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.stockPoolRepository = stockPoolRepository;
            this.palletRepository = repository;
            this.locationTypeRepository = locationTypeRepository;
            this.storageLocationTypeRepository = storageLocationTypeRepository;
        }

        protected override async Task<StoresPallet> CreateFromResourceAsync(
            StoresPalletResource resource,
            IEnumerable<string> privileges = null)
        {
            var alreadyExist = this.palletRepository.FindById(resource.PalletNumber);

            if (alreadyExist != null)
            {
                throw new AlreadyExistsException($"Pallet {resource.PalletNumber} already exists.");
            }

            var stockPool = this.stockPoolRepository.FindById(resource.DefaultStockPoolId);

            if (stockPool == null && resource.DefaultStockPoolId != null)
            {
                throw new NullReferenceException($"Stock pool {resource.DefaultStockPool.StockPoolCode} not found.");
            }

            var locationType = this.locationTypeRepository.FindById(resource.LocationTypeId);

            if (locationType == null && !string.IsNullOrEmpty(resource.LocationTypeId))
            {
                throw new NullReferenceException($"Location type {resource.LocationTypeId} not found.");
            }

            var storageLocation = this.storageLocationTypeRepository.FindById(resource.LocationIdCode);

            if (storageLocation == null)
            {
                throw new NullReferenceException($"Storage location {resource.LocationIdCode} not found.");
            }

            return new StoresPallet(
                resource.PalletNumber,
                resource.Description,
                storageLocation,
                DateTime.Parse(resource.DateInvalid),
                DateTime.Parse(resource.DateLastAudited),
                resource.Accessible,
                resource.StoresKittable,
                resource.StoresKittablePriority,
                resource.SalesKittable,
                resource.SalesKittablePriority,
                DateTime.Parse(resource.AllocQueueTime),
                locationType,
                resource.AuditedBy,
                stockPool,
                resource.StockType,
                resource.StockState,
                resource.AuditOwnerId,
                resource.AuditFrequencyWeeks,
                resource.AuditedByDepartmentCode,
                resource.MixStates,
                resource.Cage);
        }

        protected override async Task UpdateFromResourceAsync(
            StoresPallet entity,
            StoresPalletResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var stockPool = this.stockPoolRepository.FindById(updateResource?.DefaultStockPoolId);

            if (stockPool == null && updateResource.DefaultStockPoolId != null)
            {
                throw new NullReferenceException($"Stock pool {updateResource?.DefaultStockPoolId} not found.");
            }

            var locationType = this.locationTypeRepository.FindById(updateResource.LocationTypeId);

            if (locationType == null && !string.IsNullOrEmpty(updateResource.LocationTypeId))
            {
                throw new NullReferenceException($"Location type {updateResource.LocationTypeId} not found.");
            }

            var storageLocation = this.storageLocationTypeRepository.FindById(updateResource.LocationIdCode);

            if (storageLocation == null)
            {
                throw new NullReferenceException($"Storage location {updateResource.LocationIdCode} not found.");
            }

            entity.Update(
                updateResource.Description,
                storageLocation, 
                DateTime.Parse(updateResource.DateInvalid),
                DateTime.Parse(updateResource.DateLastAudited),
                updateResource.Accessible,
                updateResource.StoresKittable,
                updateResource.StoresKittablePriority,
                updateResource.SalesKittable,
                updateResource.SalesKittablePriority,
                DateTime.Parse(updateResource.AllocQueueTime),
                locationType,
                updateResource.AuditedBy,
                stockPool,
                updateResource.StockType,
                updateResource.StockState,
                updateResource.AuditOwnerId,
                updateResource.AuditFrequencyWeeks,
                updateResource.AuditedByDepartmentCode,
                updateResource.MixStates,
                updateResource.Cage);
        }

        protected override Expression<Func<StoresPallet, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            StoresPallet entity,
            StoresPalletResource resource,
            StoresPalletResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            StoresPallet entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresPallet, bool>> FilterExpression(StoresPalletResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresPallet, bool>> FindExpression(StoresPalletResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
