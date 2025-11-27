namespace Linn.Stores2.Domain.LinnApps.Pcas
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class PcasStorageType
    {
        public PcasStorageType()
        {
        }

        public PcasStorageType(
            PcasBoard pcasBoard,
            StorageType storageType,
            int? maximum,
            int? increment,
            string remarks,
            string preference)
        {
            this.BoardCode = pcasBoard.BoardCode;
            this.StorageTypeCode = storageType.StorageTypeCode;
            this.Maximum = maximum;
            this.Increment = increment;
            this.Remarks = remarks;
            this.Preference = preference;
        }

        public PcasStorageTypeKey Key { get; set; }

        public string BoardCode { get; set; }

        public string StorageTypeCode { get; set; }

        public int? Maximum { get; set; }

        public int? Increment { get; set; }

        public string Remarks { get; set; }

        public string Preference { get; set; }

        public StorageType StorageType { get; set; }

        public PcasBoard PcasBoard { get; set; }

        public void Update(
            int? maximum,
            int? increment,
            string remarks,
            string preference)
        {
            this.Maximum = maximum;
            this.Increment = increment;
            this.Remarks = remarks;
            this.Preference = preference;
        }

        public void ValidateUpdateAndCreate(
            PcasStorageType partStorageTypeAlreadyExists,
            PcasStorageType preferenceAlreadyExists,
            string updatePreference,
            string createOrUpdate,
            StorageType storageType,
            PcasBoard pcasBoard)
        {
            if (createOrUpdate == "create" && partStorageTypeAlreadyExists != null)
            {
                throw new PcasStorageTypeException("Part Storage Type Already Exists");
            }

            if (preferenceAlreadyExists != null && (createOrUpdate == "create" || preferenceAlreadyExists.StorageTypeCode != this.StorageTypeCode))
            {
                throw new PcasStorageTypeException("Part Preference Already Exists");
            }

            if (string.IsNullOrEmpty(updatePreference))
            {
                throw new PcasStorageTypeException("Part Preference is Empty");
            }

            if (storageType == null)
            {
                throw new PcasStorageTypeException("Storage Type doesn't exist");
            }

            if (pcasBoard == null)
            {
                throw new PcasStorageTypeException("PCAS Board doesn't exist");
            }
        }
    }
}
