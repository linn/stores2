namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Pcas;

    public class PcasStorageTypeFacadeService : AsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource>
    {
        public PcasStorageTypeFacadeService(
            IRepository<PcasStorageType, PcasStorageTypeKey> repository,
            ITransactionManager transactionManager,
            IBuilder<PcasStorageType> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override async Task<PcasStorageType> CreateFromResourceAsync(
            PcasStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            return new PcasStorageType(
                resource.BoardCode,
                resource.StorageTypeCode,
                resource.Maximum,
                resource.Increment,
                resource.Remarks,
                resource.Preference);
        }

        protected override async Task UpdateFromResourceAsync(
            PcasStorageType entity,
            PcasStorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.Update(
                updateResource.Maximum,
                updateResource.Increment,
                updateResource.Remarks,
                updateResource.Preference);
        }

        protected override Task SaveToLogTable(string actionType, int userNumber, PcasStorageType entity, PcasStorageTypeResource resource, PcasStorageTypeResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PcasStorageType entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PcasStorageType, bool>> FilterExpression(PcasStorageTypeResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PcasStorageType, bool>> FindExpression(PcasStorageTypeResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PcasStorageType, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
