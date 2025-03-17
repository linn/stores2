namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StorageTypeFacadeService : AsyncFacadeService<StorageType, string, StorageTypeResource, StorageTypeResource, StorageTypeResource>
    {
        private readonly IRepository<StorageType, string> repository;

        public StorageTypeFacadeService(
            IRepository<StorageType, string> repository,
            ITransactionManager transactionManager,
            IBuilder<StorageType> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.repository = repository;
        }

        protected override async Task<StorageType> CreateFromResourceAsync(
            StorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            var storageType = await this.repository.FindByIdAsync(resource.StorageTypeCode);

            if (storageType != null)
            {
                throw new StorageTypeException("Storage Type Code already exists!");
            }

            return new StorageType(
                            resource.StorageTypeCode,
                            resource.Description);
        }

        protected override void UpdateFromResource(
            StorageType entity,
            StorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.Update(updateResource.Description);
        }

        protected override Expression<Func<StorageType, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            StorageType entity,
            StorageTypeResource resource,
            StorageTypeResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            StorageType entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StorageType, bool>> FilterExpression(StorageTypeResource searchResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StorageType, bool>> FindExpression(StorageTypeResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}

