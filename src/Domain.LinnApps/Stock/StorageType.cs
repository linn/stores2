namespace Linn.Stores2.Domain.LinnApps.Stock
{
    public class StorageType
    {
        public StorageType()
        {
        }
        public StorageType(
            string storageTypeCode,
            string description)
        {
            this.StorageTypeCode = storageTypeCode.ToUpper().Trim();
            this.Description = description;
        }
        public string StorageTypeCode { get; set; }

        public string Description { get; set; }
        public void Update(string description)
        {
            this.Description = description;
        }
    }
}
