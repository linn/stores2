namespace Linn.Stores2.Domain.LinnApps
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StorageTypesService : IStorageTypeService
    {
        private readonly IRepository<PartsStorageType, int> partStorageTypeRepository;

        private readonly IRepository<PcasStorageType, PcasStorageTypeKey> pcasStorageTypeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageType, string> storageTypeRepository;

        private readonly IRepository<PcasBoard, string> pcasBoardRepository;

        private readonly ITransactionManager transactionManager;

        public StorageTypesService(
            IRepository<PartsStorageType, int> partStorageTypeRepository,
            IRepository<PcasStorageType, PcasStorageTypeKey> pcasStorageTypeRepository,
            ITransactionManager transactionManager,
            IRepository<Part, string> partRepository,
            IRepository<StorageType, string> storageTypeRepository,
            IRepository<PcasBoard, string> pcasBoardRepository)
        {
            this.partRepository = partRepository;
            this.pcasStorageTypeRepository = pcasStorageTypeRepository;
            this.storageTypeRepository = storageTypeRepository;
            this.partStorageTypeRepository = partStorageTypeRepository;
            this.transactionManager = transactionManager;
            this.pcasBoardRepository = pcasBoardRepository;
        }

        public async Task<PartsStorageType> ValidatePartsStorageType(PartsStorageType partStorageType)
        {
            if (string.IsNullOrEmpty(partStorageType.Preference) || partStorageType.Preference == "0")
            {
                throw new PartsStorageTypeException("Part Preference is Empty or 0");
            }

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

            return new PartsStorageType(
                part,
                storageType,
                partStorageType.Remarks,
                partStorageType.Maximum,
                partStorageType.Incr,
                partStorageType.Preference,
                partStorageType.BridgeId);
        }

        public async Task<PcasStorageType> ValidateCreatePcasStorageType(PcasStorageType pcasStorageType)
        {
            var pcasBoard = await this.pcasBoardRepository.FindByIdAsync(pcasStorageType?.BoardCode);

            var storageType = await this.storageTypeRepository.FindByIdAsync(pcasStorageType.StorageTypeCode);

            var pcasStorageTypeAlreadyExists = await this.pcasStorageTypeRepository.FindByAsync(
                                                   pst => pst.StorageTypeCode == pcasStorageType.StorageTypeCode
                                                          && pst.BoardCode == pcasStorageType.BoardCode);

            if (pcasStorageTypeAlreadyExists != null)
            {
                throw new PcasStorageTypeException("Pcas Storage Type Already Exists");
            }

            var pcasPreferenceAlreadyExists = await this.pcasStorageTypeRepository.FindByAsync(
                                                  pst => pst.Preference == pcasStorageType.Preference
                                                         && pst.BoardCode == pcasStorageType.BoardCode);

            if (pcasPreferenceAlreadyExists != null && (pcasPreferenceAlreadyExists.Key?.Equals(pcasStorageType.Key) ?? false))
            {
                throw new PcasStorageTypeException("Pcas Storage Type Preference Already Exists");
            }

            return new PcasStorageType(
                pcasBoard,
                storageType,
                pcasStorageType.Maximum,
                pcasStorageType.Increment,
                pcasStorageType.Remarks,
                pcasStorageType.Preference);
        }

        public async Task<PcasStorageType> ValidateUpdatePcasStorageType(PcasStorageType pcasStorageType)
        {
            var pcasBoard = await this.pcasBoardRepository.FindByIdAsync(pcasStorageType?.BoardCode);

            var storageType = await this.storageTypeRepository.FindByIdAsync(pcasStorageType.StorageTypeCode);

            var pcasPreferenceAlreadyExists = await this.pcasStorageTypeRepository.FindByAsync(
                                                  pst => pst.Preference == pcasStorageType.Preference
                                                         && pst.BoardCode == pcasStorageType.BoardCode);

            if (pcasPreferenceAlreadyExists != null)
            {
                throw new PcasStorageTypeException("Pcas Storage Type Preference Already Exists");
            }

            return new PcasStorageType(
                pcasBoard,
                storageType,
                pcasStorageType.Maximum,
                pcasStorageType.Increment,
                pcasStorageType.Remarks,
                pcasStorageType.Preference);
        }
    }
}
