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
            if (string.IsNullOrEmpty(preference) || preference == "0")
            {
                throw new PcasStorageTypeException("Pcas Storage Type Preference is Empty or 0");
            } 

            this.Key = new PcasStorageTypeKey
                           {
                               BoardCode = pcasBoard?.BoardCode ?? throw new PartStorageTypeException("Part Number is empty or doesn't exist!"),
                               StorageTypeCode = storageType.StorageTypeCode ?? throw new PartStorageTypeException("Storage Type is empty or doesn't exist!")
            };
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
    }
}
