namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Pcas;

    public class PcasStorageTypeResourceBuilder : IBuilder<PcasStorageType>
    {
        public PcasStorageTypeResource Build(PcasStorageType pcasStorageType, IEnumerable<string> claims)
        {
            return new PcasStorageTypeResource
            {
                BoardCode = pcasStorageType.BoardCode,
                StorageTypeCode = pcasStorageType.StorageTypeCode,
                Incr = pcasStorageType.Incr,
                Maximum = pcasStorageType.Maximum,
                Remarks = pcasStorageType.Remarks,
                Preference = pcasStorageType.Preference
            };
        }

        public string GetLocation(PcasStorageType model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<PcasStorageType>.Build(PcasStorageType entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PcasStorageType model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
