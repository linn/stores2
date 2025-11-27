namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using System;
    using System.Threading.Tasks;

    public class StorageTypesService : IStorageTypeService
    {
        private readonly IRepository<PartsStorageType, int> partStorageTypeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageType, string> storageTypeRepository;

        private readonly ITransactionManager transactionManager;

        public StorageTypesService(
            IRepository<PartsStorageType, int> partStorageTypeRepository,
            ITransactionManager transactionManager,
            IRepository<Part, string> partRepository,
            IRepository<StorageType, string> storageTypeRepository)
        {
            this.partRepository = partRepository;
            this.storageTypeRepository = storageTypeRepository;
            this.partStorageTypeRepository = partStorageTypeRepository;
            this.transactionManager = transactionManager;
        }

        public async Task<PartsStorageType> ValidatePartsStorageType(PartsStorageType partStorageType)
        {
            var part = await this.partRepository.FindByIdAsync(partStorageType.PartNumber);

            var storageType = await this.storageTypeRepository.FindByIdAsync(partStorageType.StorageTypeCode);

            var partStorageTypeAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                                                   pst => pst.StorageTypeCode == partStorageType.StorageTypeCode
                                                          && pst.PartNumber == partStorageType.PartNumber);

            if (partStorageTypeAlreadyExists != null && partStorageType.BridgeId == 0) 
            { 
                throw new PartsStorageTypeException("Part Storage Type Already Exists");
            }

            var partPreferenceAlreadyExists = await this.partStorageTypeRepository.FindByAsync(
                                                  pst => pst.Preference == partStorageType.Preference
                                                         && pst.PartNumber == partStorageType.PartNumber);

            if (partPreferenceAlreadyExists != null && (partPreferenceAlreadyExists.BridgeId != partStorageType.BridgeId))
            {
                throw new PartsStorageTypeException("Part Preference Already Exists");
            }

            if (string.IsNullOrEmpty(partStorageType.Preference))
            {
                throw new PartsStorageTypeException("Part Preference is Empty");
            }

            return new PartsStorageType(
                part,
                storageType,
                partStorageType.Remarks,
                partStorageType.Maximum,
                partStorageType.Incr,
                partStorageType.Preference,
                partStorageType.BridgeId);
        }

        public async Task<PcasStorageType> ValidatePcasStorageType(PcasStorageType pcasStorageType)
        {

            throw new NotImplementedException();
        }
    }
}
