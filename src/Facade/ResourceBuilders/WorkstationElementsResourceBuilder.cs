namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Stores;

    public class WorkStationElementsResourceBuilder : IBuilder<WorkStationElement>
    {
        public WorkStationElementResource Build(WorkStationElement model, IEnumerable<string> claims)
        {
            var claimsList = claims?.ToList() ?? new List<string>();

            return new WorkStationElementResource
            {
                WorkStationElementId = model.WorkStationElementId,
                WorkStationCode = model.WorkStationCode,
                PalletNumber = model.Pallet?.PalletNumber,
                LocationId = model.StorageLocation?.LocationId,
                LocationCode = model.StorageLocation?.LocationCode,
                LocationDescription = model.StorageLocation?.Description,
                DateCreated = model.DateCreated.ToString("o"),
                CreatedById = model.CreatedBy?.Id,
                CreatedByName = model.CreatedBy?.Name,
                Links = this.BuildLinks(model, claimsList).ToArray()
            };
        }

        public string GetLocation(WorkStationElement model)
        {
            return $"/stores2/work-stations/{model.WorkStationCode}/{model.WorkStationElementId}";
        }

        object IBuilder<WorkStationElement>.Build(WorkStationElement entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(WorkStationElement model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
