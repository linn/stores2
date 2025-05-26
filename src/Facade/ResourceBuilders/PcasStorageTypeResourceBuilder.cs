namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Pcas;

    public class PcasStorageTypeResourceBuilder : IBuilder<PcasStorageType>
    {
        public PcasStorageTypeResource Build(PcasStorageType pcasStorageType, IEnumerable<string> claims)
        {
            return new PcasStorageTypeResource
            {
                BoardCode = pcasStorageType.BoardCode,
                PcasBoard = new PcasBoardResource
                                {
                                    BoardCode = pcasStorageType.PcasBoard.BoardCode,
                                    Description = pcasStorageType.PcasBoard.Description
                                },
                StorageTypeCode = pcasStorageType.StorageTypeCode,
                StorageType = new StorageTypeResource
                                  {
                                      StorageTypeCode = pcasStorageType.StorageType.StorageTypeCode,
                                      Description = pcasStorageType.StorageType.Description
                                  },
                Increment = pcasStorageType.Increment,
                Maximum = pcasStorageType.Maximum,
                Remarks = pcasStorageType.Remarks,
                Preference = pcasStorageType.Preference,
                Links = this.BuildLinks(pcasStorageType, claims).ToArray()
            };
        }

        public string GetLocation(PcasStorageType model)
        {
            return $"/stores2/pcas-storage-types/{model.BoardCode}/{model.StorageTypeCode}";
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
