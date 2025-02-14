namespace Linn.Stores2.Domain.LinnApps
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class PartsStorageType
    {
        public PartsStorageType()
        {
        }

        public PartsStorageType(Part part, StorageType storageType, string remarks, int maximum, int incr, string preference, int bridgeId)
        {
            this.Part = part ?? throw new PartsStorageTypeException("Part Number is empty or doesn't exist!");
            this.PartNumber = part.PartNumber;
            this.StorageType = storageType ?? throw new PartsStorageTypeException("Storage Type is empty or doesn't exist!");
            this.StorageTypeCode = storageType.StorageTypeCode;
            this.Remarks = remarks;
            this.Maximum = maximum;
            this.Incr = incr;
            this.Preference = preference;
            this.BridgeId = bridgeId;
        }

        public string PartNumber { get; set; }

        public Part Part { get; set; }

        public string StorageTypeCode { get; set; }

        public StorageType StorageType { get; set; }

        public string Remarks { get; set; }

        public int Maximum { get; set; }

        public int Incr { get; set; }

        public string Preference { get; set; }

        public int BridgeId { get; set; }

        public void Update(string remarks, int maximum, int incr, string preference, int bridgeId)
        {
            this.Remarks = remarks;
            this.Maximum = maximum;
            this.Incr = incr;
            this.Preference = preference;
            this.BridgeId = bridgeId;
        }
    }
}
