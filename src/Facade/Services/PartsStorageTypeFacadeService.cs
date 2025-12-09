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

        private readonly IStorageTypeService storageTypeService;

        private readonly IDatabaseService databaseService;

        public PartsStorageTypeFacadeService(
            IRepository<PartsStorageType, int> partStorageTypeRepository,
            ITransactionManager transactionManager,
            IBuilder<PartsStorageType> resourceBuilder,
            IRepository<Part, string> partRepository,
            IRepository<StorageType, string> storageTypeRepository,
            IDatabaseService databaseService,
            IStorageTypeService storageTypeService)
            : base(partStorageTypeRepository, transactionManager, resourceBuilder)
        {
            this.partRepository = partRepository;
            this.storageTypeRepository = storageTypeRepository;
            this.partStorageTypeRepository = partStorageTypeRepository;
            this.databaseService = databaseService;
            this.storageTypeService = storageTypeService;
        }

        protected override async Task<PartsStorageType> CreateFromResourceAsync(
            PartsStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            var entity = new PartsStorageType(
                new Part { PartNumber = resource.PartNumber },
                new StorageType { StorageTypeCode = resource.StorageTypeCode },
                resource.Remarks,
                resource.Maximum,
                resource.Incr,
                resource.Preference,
                0);

            var validatedPartsStorageType = await this.storageTypeService.ValidatePartsStorageType(entity);

            var bridgeId = this.databaseService.GetIdSequence("PARTS_STORAGE_TYPES_ID_SEQ");

            return new PartsStorageType(
                validatedPartsStorageType.Part,
                validatedPartsStorageType.StorageType,
                validatedPartsStorageType.Remarks,
                validatedPartsStorageType.Maximum,
                validatedPartsStorageType.Incr,
                validatedPartsStorageType.Preference,
                bridgeId);
        }

        protected override async Task UpdateFromResourceAsync(
            PartsStorageType entity,
            PartsStorageTypeResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var partsStorageType = new PartsStorageType(
                new Part { PartNumber = updateResource.PartNumber },
                new StorageType { StorageTypeCode = updateResource.StorageTypeCode },
                updateResource.Remarks,
                updateResource.Maximum,
                updateResource.Incr,
                updateResource.Preference,
                updateResource.BridgeId);

            var validatedPartsStorageType = await this.storageTypeService.ValidatePartsStorageType(partsStorageType);

            entity.Update(
                validatedPartsStorageType.Remarks,
                validatedPartsStorageType.Maximum,
                validatedPartsStorageType.Incr,
                validatedPartsStorageType.Preference);
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

            this.partStorageTypeRepository.Remove(partStorageType);
        }

        protected override Expression<Func<PartsStorageType, bool>> FilterExpression(PartsStorageTypeResource searchResource)
        {
            return x =>
                (string.IsNullOrEmpty(searchResource.PartNumber) || x.PartNumber.ToUpper().Contains(searchResource.PartNumber.ToUpper())) &&
                (string.IsNullOrEmpty(searchResource.StorageTypeCode) || x.StorageTypeCode.ToUpper().Contains(searchResource.StorageTypeCode.ToUpper()));
        }

        protected override Expression<Func<PartsStorageType, bool>> FindExpression(PartsStorageTypeResource searchResource)
        {
            return x => x.PartNumber == searchResource.PartNumber && x.StorageTypeCode == searchResource.StorageTypeCode;
        }
    }
}