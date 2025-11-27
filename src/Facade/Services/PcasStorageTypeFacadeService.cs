namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources.Pcas;

    public class PcasStorageTypeFacadeService : AsyncFacadeService<PcasStorageType, PcasStorageTypeKey, PcasStorageTypeResource, PcasStorageTypeResource, PcasStorageTypeResource>
    {
        private readonly IRepository<PcasStorageType, PcasStorageTypeKey> repository;

        private readonly IRepository<StorageType, string> storageTypeRepository;

        private readonly IRepository<PcasBoard, string> pcasBoardRepository;

        public PcasStorageTypeFacadeService(
            IRepository<PcasStorageType, PcasStorageTypeKey> repository,
            ITransactionManager transactionManager,
            IBuilder<PcasStorageType> resourceBuilder,
            IRepository<StorageType, string> storageTypeRepository,
            IRepository<PcasBoard, string> pcasBoardRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.storageTypeRepository = storageTypeRepository;
            this.pcasBoardRepository = pcasBoardRepository;
            this.repository = repository;
        }

        protected override async Task<PcasStorageType> CreateFromResourceAsync(
            PcasStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        { 
            var pcasBoard = await this.pcasBoardRepository.FindByIdAsync(resource.BoardCode);

            var storageType = await this.storageTypeRepository.FindByIdAsync(resource.StorageTypeCode);

            var pcasStorageTypeAlreadyExists = await this.repository.FindByAsync(
                                                   pst => pst.StorageTypeCode == resource.StorageTypeCode
                                                          && pst.BoardCode == resource.BoardCode);


            var pcasPreferenceAlreadyExists = await this.repository.FindByAsync(
                                                  pst => pst.Preference == resource.Preference
                                                         && pst.BoardCode == resource.BoardCode);

            var entity = new PcasStorageType();

            entity.ValidateUpdateAndCreate(pcasStorageTypeAlreadyExists, pcasPreferenceAlreadyExists, resource.Preference, "create",storageType, pcasBoard);

            return new PcasStorageType(
                pcasBoard,
                storageType,
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
            var pcasBoard = await this.pcasBoardRepository.FindByIdAsync(updateResource.BoardCode);

            var storageType = await this.storageTypeRepository.FindByIdAsync(updateResource.StorageTypeCode);

            var pcasPreferenceAlreadyExists = await this.repository.FindByAsync(
                                                  pst => pst.Preference == updateResource.Preference
                                                         && pst.BoardCode == updateResource.BoardCode);

            entity.ValidateUpdateAndCreate(null, pcasPreferenceAlreadyExists, updateResource.Preference, "update", storageType, pcasBoard);

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
