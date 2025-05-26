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
    using Linn.Stores2.Facade.Common;
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
            var alreadyExists = await this.repository.FindByIdAsync(
                new PcasStorageTypeKey { BoardCode = resource.BoardCode, StorageTypeCode = resource.StorageTypeCode });

            if (alreadyExists != null)
            {
                throw new PcasStorageTypeException("This PCAS Storage Type already exists");
            }


            var pcasBoard = await this.pcasBoardRepository.FindByIdAsync(resource.BoardCode);

            if (pcasBoard == null)
            {
                throw new PcasStorageTypeException("The PCAS Board does not exist");
            }

            var storageType = await this.storageTypeRepository.FindByIdAsync(resource.StorageTypeCode);


            if (storageType == null)
            {
                throw new PcasStorageTypeException("The Storage Type does not exist");
            }

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
