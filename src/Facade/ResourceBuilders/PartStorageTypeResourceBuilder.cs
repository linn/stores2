namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Parts;

    public class PartStorageTypeResourceBuilder : IBuilder<PartStorageType>
    {
        public PartStorageTypeResource Build(PartStorageType partsStorageType, IEnumerable<string> claims)
        {
            return new PartStorageTypeResource
                       {
                           StorageTypeCode = partsStorageType.StorageTypeCode,
                           StorageType = new StorageTypeResource
                           {
                               StorageTypeCode = partsStorageType.StorageType.StorageTypeCode,
                               Description = partsStorageType.StorageType.Description
                           },
                           PartNumber = partsStorageType.PartNumber,
                           Part = new PartResource 
                            {
                               PartNumber = partsStorageType.Part.PartNumber,
                               Description = partsStorageType.Part.Description
                            },
                           Remarks = partsStorageType.Remarks,
                           Maximum = partsStorageType.Maximum,
                           BridgeId = partsStorageType.BridgeId,
                           Incr = partsStorageType.Incr,
                           Preference = partsStorageType.Preference,
                           Links = this.BuildLinks(partsStorageType, claims).ToArray()
                       };
        }

        public string GetLocation(PartStorageType model)
        {
            return $"/stores2/part-storage-types/{model.BridgeId}";
        }

        object IBuilder<PartStorageType>.Build(PartStorageType entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PartStorageType model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
