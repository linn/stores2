namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Amazon.Auth.AccessControlPolicy;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    public class PartsStorageTypeFacadeService : AsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource>
    {
        public PartsStorageTypeFacadeService(
            IRepository<PartsStorageType, PartsStorageTypeKey> repository,
            ITransactionManager transactionManager,
            IBuilder<PartsStorageType> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override async Task<PartsStorageType> CreateFromResourceAsync(
            PartsStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            return new PartsStorageType(
                resource.PartNumber,
                resource.StorageTypeCode,
                resource.Remarks,
                resource.Maximum,
                resource.Incr,
                resource.Preference,
                resource.BridgeId);
        }

        protected override async Task UpdateFromResourceAsync(
            PartsStorageType entity,
            PartsStorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.Update(
                updateResource.PartNumber,
                updateResource.StorageTypeCode,
                updateResource.Remarks,
                updateResource.Maximum,
                updateResource.Incr,
                updateResource.Preference,
                updateResource.BridgeId);
        }

        protected override Expression<Func<PartsStorageType, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            PartsStorageType entity,
            PartsStorageTypeResource resource,
            PartsStorageTypeResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PartsStorageType entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PartsStorageType, bool>> FilterExpression(PartsStorageTypeResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PartsStorageType, bool>> FindExpression(PartsStorageTypeResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}