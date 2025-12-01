namespace Linn.Stores2.Domain.LinnApps
{
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Pcas;

    public interface IStorageTypeService
    {
        Task<PartsStorageType> ValidatePartsStorageType(PartsStorageType partsStorageType);

        Task<PcasStorageType> ValidateCreatePcasStorageType(PcasStorageType pcasStorageType);

        Task<PcasStorageType> ValidateUpdatePcasStorageType(PcasStorageType pcasStorageType);
    }
}

