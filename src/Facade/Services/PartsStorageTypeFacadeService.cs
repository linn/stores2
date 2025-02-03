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
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class PartsStorageTypeFacadeService : AsyncFacadeService<PartsStorageType, PartsStorageTypeKey, PartsStorageTypeResource, PartsStorageTypeResource, PartsStorageTypeResource>
    {
        private readonly IRepository<Part, int> partRepository;

        private readonly IRepository<StorageType, string> storageTypeRepository;

        private readonly IRepository<PartsStorageType, PartsStorageTypeKey> partStorageTypeRepository;

        public PartsStorageTypeFacadeService(
            IRepository<PartsStorageType, PartsStorageTypeKey> partStorageTypeRepository,
            ITransactionManager transactionManager,
            IBuilder<PartsStorageType> resourceBuilder,
            IRepository<Part, int> partRepository,
            IRepository<StorageType, string> storageTypeRepository)
            : base(partStorageTypeRepository, transactionManager, resourceBuilder)
        {
            this.partRepository = partRepository;
            this.storageTypeRepository = storageTypeRepository;
        }

        protected override async Task<PartsStorageType> CreateFromResourceAsync(
            PartsStorageTypeResource resource,
            IEnumerable<string> privileges = null)
        {
            var part = await this.partRepository.FindByAsync(p => p.PartNumber == resource.PartNumber);

            if (part == null || string.IsNullOrEmpty(resource.PartNumber))
            {
                throw new PartsStorageTypeException("Part Number is empty or doesn't exist!");
            }

            var storageType = await this.storageTypeRepository.FindByAsync(st => st.StorageTypeCode == resource.StorageTypeCode);

            if (storageType == null || string.IsNullOrEmpty(resource.StorageTypeCode))
            {
                throw new PartsStorageTypeException("Storage Type is empty or doesn't exist!");
            }

            var partStorageTypeAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                pst => pst.StorageTypeCode == resource.StorageTypeCode && pst.PartNumber == resource.PartNumber);

            if (partStorageTypeAlreadyExists != null)
            {
                throw new PartsStorageTypeException("Part Storage Type Already Exists");
            }

            return new PartsStorageType(
                part,
                storageType,
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
            return x => x.PartNumber == searchResource.PartNumber && x.StorageTypeCode == searchResource.StorageTypeCode;
        }
    }
}