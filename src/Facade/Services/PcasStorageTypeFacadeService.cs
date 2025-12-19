namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources.Pcas;

    public class PcasStorageTypeFacadeService : AsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource>
    {
        private readonly IStorageTypeService storageTypeService;

        public PcasStorageTypeFacadeService(
            IRepository<PcasStorageType, PcasStorageTypeKey> repository,
            ITransactionManager transactionManager,
            IBuilder<PcasStorageType> resourceBuilder,
            IStorageTypeService storageTypeService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.storageTypeService = storageTypeService;
        }

        protected override async Task<PcasStorageType> CreateFromResourceAsync(
            PcasStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            var entity = new PcasStorageType(
                new PcasBoard { BoardCode = resource.BoardCode },
                new StorageType { StorageTypeCode = resource.StorageTypeCode },
                resource.Maximum,
                resource.Increment,
                resource.Remarks,
                resource.Preference);

            var validatedPcasStorageType = await this.storageTypeService.ValidateCreatePcasStorageType(entity);

            return validatedPcasStorageType;
        }

        protected override async Task UpdateFromResourceAsync(
            PcasStorageType entity,
            PcasStorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var pcasStorageType = new PcasStorageType(
                new PcasBoard { BoardCode = updateResource.BoardCode },
                new StorageType { StorageTypeCode = updateResource.StorageTypeCode },
                updateResource.Maximum,
                updateResource.Increment,
                updateResource.Remarks,
                updateResource.Preference);

            var validatedPcasStorageType = await this.storageTypeService.ValidateUpdatePcasStorageType(pcasStorageType);

            entity.Update(
                validatedPcasStorageType.Maximum,
                validatedPcasStorageType.Increment,
                validatedPcasStorageType.Remarks,
                validatedPcasStorageType.Preference);
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
