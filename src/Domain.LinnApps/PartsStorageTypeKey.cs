namespace Linn.Stores2.Domain.LinnApps
{
    public class PartsStorageTypeKey
    {
        public PartsStorageTypeKey(string partNumber, string storageTypeCode)
        {
            this.PartNumber = partNumber;
            this.StorageTypeCode = storageTypeCode;
        }

        public string PartNumber { get; set; }

        public string StorageTypeCode { get; set; }

    }
}
