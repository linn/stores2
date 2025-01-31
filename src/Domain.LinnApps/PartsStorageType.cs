namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using Linn.Stores2.Domain.LinnApps.Parts;

    public class PartsStorageType
    {
        public PartsStorageType()
        {
        }

        public PartsStorageType(Part part, StorageType storageType, string remarks, int maximum, int incr, string preference, int bridgeId)
        {
            this.Key = new PartsStorageTypeKey(part.PartNumber, storageType.StorageTypeCode);
            this.Part = part;
            this.StorageType = storageType;
            this.Remarks = remarks;
            this.Maximum = maximum;
            this.Incr = incr;
            this.Preference = preference;
            this.BridgeId = bridgeId;
        }

        public PartsStorageTypeKey Key { get; set; }

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
