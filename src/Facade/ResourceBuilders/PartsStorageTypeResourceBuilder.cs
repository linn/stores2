namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources.Parts;

    public class PartsStorageTypeResourceBuilder : IBuilder<PartsStorageType>
    {
        public PartsStorageTypeResource Build(PartsStorageType partsStorageType, IEnumerable<string> claims)
        {
            return new PartsStorageTypeResource()
                       {
                           StorageTypeCode = partsStorageType.StorageTypeCode,
                           PartNumber = partsStorageType.PartNumber,
                           Remarks = partsStorageType.Remarks,
                           Maximum = partsStorageType.Maximum,
                           BridgeId = partsStorageType.BridgeId,
                           Incr = partsStorageType.Incr,
                           Preference = partsStorageType.Preference,
                           Links = this.BuildLinks(partsStorageType, claims).ToArray()
                       };
        }

        public string GetLocation(PartsStorageType model)
        {
            return $"/stores2/parts-storage-types/{model.StorageTypeCode}/{model.PartNumber}";
        }

        object IBuilder<PartsStorageType>.Build(PartsStorageType entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PartsStorageType model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
