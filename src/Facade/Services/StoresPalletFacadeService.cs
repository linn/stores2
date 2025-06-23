namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StoresPalletFacadeService : AsyncFacadeService<StoresPallet, int, StoresPalletResource, StoresPalletResource, StoresPalletResource>
    {
        private readonly IRepository<StockPool, string> stockPoolRepository;

        private readonly IQueryRepository<LocationType> locationTypeRepository;

        private readonly IRepository<StorageLocation, int> storageLocationTypeRepository;

        private readonly IRepository<StoresPallet, int> palletRepository;

        public StoresPalletFacadeService(
            IRepository<StoresPallet, int> repository,
            ITransactionManager transactionManager,
            IBuilder<StoresPallet> resourceBuilder,
            IRepository<StockPool, string> stockPoolRepository,
            IQueryRepository<LocationType> locationTypeRepository,
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
            var alreadyExist = await this.palletRepository.FindByIdAsync(resource.PalletNumber);

            if (alreadyExist != null)
            {
                throw new StoresPalletException($"Pallet {resource.PalletNumber} already exists.");
            }

            var stockPool = await this.stockPoolRepository.FindByIdAsync(resource.DefaultStockPoolId);

            var locationType = await this.locationTypeRepository.FilterByAsync(x => x.Code == resource.LocationTypeId);

            var storageLocation = await this.storageLocationTypeRepository.FindByIdAsync(resource.StorageLocationId);

            return new StoresPallet(
                resource.PalletNumber,
                resource.Description,
                storageLocation,
                resource.StorageLocationId,
                resource.Accessible,
                resource.StoresKittable,
                resource.StoresKittingPriority,
                resource.SalesKittable,
                resource.SalesKittingPriority,
                DateTime.Parse(resource.AllocQueueTime),
                locationType.FirstOrDefault(),
                resource.LocationTypeId == "LINN" ? "L" : null,
                resource.AuditedBy,
                stockPool,
                resource.DefaultStockPoolId,
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
            var stockPool = await this.stockPoolRepository.FindByIdAsync(updateResource?.DefaultStockPoolId);

            var locationType = await this.locationTypeRepository.FilterByAsync(x => x.Code == updateResource.LocationTypeId);

            var storageLocation = await this.storageLocationTypeRepository.FindByIdAsync(updateResource.StorageLocationId);

            DateTime? dateInvalid = null;
            if (!string.IsNullOrWhiteSpace(updateResource.DateInvalid) && DateTime.TryParse(updateResource.DateInvalid, out var parsedDateInvalid))
            {
                dateInvalid = parsedDateInvalid;
            }

            DateTime? dateLastAudited = null;
            if (!string.IsNullOrWhiteSpace(updateResource.DateLastAudited) && DateTime.TryParse(updateResource.DateLastAudited, out var parsedDateLastAudited))
            {
                dateLastAudited = parsedDateLastAudited;
            }

            entity.Update(
                updateResource.Description,
                storageLocation, 
                updateResource.StorageLocationId,
                dateInvalid,
                dateLastAudited, 
                updateResource.Accessible,
                updateResource.StoresKittable,
                updateResource.StoresKittingPriority,
                updateResource.SalesKittable,
                updateResource.SalesKittingPriority,
                DateTime.Parse(updateResource.AllocQueueTime),
                locationType.FirstOrDefault(),
                updateResource.LocationTypeId,
                updateResource.AuditedBy,
                stockPool,
                updateResource.DefaultStockPoolId,
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
            bool isInt = int.TryParse(searchTerm, out int palletNumber);

            return x => isInt && x.PalletNumber == palletNumber;
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
