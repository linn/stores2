namespace Linn.Stores2.Domain.LinnApps.Stock
{
    using System.Collections.Generic;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class StorageSite
    {
        // for EF Core
        public StorageSite()
        {
        }

        public StorageSite(string code, string description, string prefix)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new InvalidEntityException("Code must be populated");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidEntityException("Description must be populated");
            }

            this.Code = code;
            this.Description = description;
            this.Prefix = prefix;
        }

        public string Code { get; protected init; }

        public string Description { get; protected set; }

        public string Prefix { get; protected set; }

        public ICollection<StorageArea> StorageAreas { get; set; }

        // todo - allow updating of StorageAreas list
        public void Update(string description, string prefix)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidEntityException("Description must be populated");
            }

            this.Description = description;
            this.Prefix = prefix;
        }
    }
}
