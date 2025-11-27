namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps.Services;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources.Parts;

    public class PartsStorageTypeFacadeService : AsyncFacadeService<PartsStorageType, int, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource>
    {
        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageType, string> storageTypeRepository;

        private readonly IRepository<PartsStorageType, int> partStorageTypeRepository;

        private readonly ITransactionManager transactionManager;

        private readonly IDatabaseService databaseService;

        public PartsStorageTypeFacadeService(
            IRepository<PartsStorageType, int> partStorageTypeRepository,
            ITransactionManager transactionManager,
            IBuilder<PartsStorageType> resourceBuilder,
            IRepository<Part, string> partRepository,
            IRepository<StorageType, string> storageTypeRepository,
            IDatabaseService databaseService)
            : base(partStorageTypeRepository, transactionManager, resourceBuilder)
        {
            this.partRepository = partRepository;
            this.storageTypeRepository = storageTypeRepository;
            this.partStorageTypeRepository = partStorageTypeRepository;
            this.databaseService = databaseService;
            this.transactionManager = transactionManager;
        }

        protected override async Task<PartsStorageType> CreateFromResourceAsync(
            PartsStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            var part = await this.partRepository.FindByIdAsync(resource.PartNumber);

            var storageType = await this.storageTypeRepository.FindByIdAsync(resource.StorageTypeCode);

            var partStorageTypeAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                                                                pst => pst.StorageTypeCode == resource.StorageTypeCode
                                                                       && pst.PartNumber == resource.PartNumber);


            var partPreferenceAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                                                  pst => pst.Preference == resource.Preference
                                                         && pst.PartNumber == resource.PartNumber);

            var entity = new PartsStorageType();

            entity.ValidateUpdateAndCreate(partStorageTypeAlreadyExists, partPreferenceAlreadyExists, resource.Preference, "create");

            var bridgeId = this.databaseService.GetIdSequence("PARTS_STORAGE_TYPES_ID_SEQ");

            return new PartsStorageType(
                part,
                storageType,
                resource.Remarks,
                resource.Maximum,
                resource.Incr,
                resource.Preference,
                bridgeId);
        }

        protected override async Task UpdateFromResourceAsync(
            PartsStorageType entity,
            PartsStorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var partPreferenceAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                                                  pst => pst.Preference == updateResource.Preference
                                                         && pst.PartNumber == updateResource.PartNumber);

            entity.ValidateUpdateAndCreate(null, partPreferenceAlreadyExists, updateResource.Preference, "update");

            entity.Update(
                updateResource.Remarks,
                updateResource.Maximum,
                updateResource.Incr,
                updateResource.Preference);
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
            var partStorageType = this.partStorageTypeRepository.FindById(entity.BridgeId);

            partStorageType.ValidateDelete();

            this.partStorageTypeRepository.Remove(partStorageType);

            this.transactionManager.Commit();
        }

        protected override Expression<Func<PartsStorageType, bool>> FilterExpression(PartsStorageTypeResource searchResource)
        {
            return x =>
                (string.IsNullOrEmpty(searchResource.PartNumber) || x.PartNumber.ToUpper() == searchResource.PartNumber.ToUpper()) &&
                (string.IsNullOrEmpty(searchResource.StorageTypeCode) || x.StorageTypeCode.ToUpper() == searchResource.StorageTypeCode.ToUpper());
        }

        protected override Expression<Func<PartsStorageType, bool>> FindExpression(PartsStorageTypeResource searchResource)
        {
            return x => x.PartNumber == searchResource.PartNumber && x.StorageTypeCode == searchResource.StorageTypeCode;
        }
    }
}