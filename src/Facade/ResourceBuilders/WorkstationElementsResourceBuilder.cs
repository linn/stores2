﻿namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Stores;

    public class WorkstationElementsResourceBuilder : IBuilder<WorkstationElement>
    {
        public WorkstationElementResource Build(WorkstationElement model, IEnumerable<string> claims)
        {
            var claimsList = claims?.ToList() ?? new List<string>();

            return new WorkstationElementResource
            {
                WorkstationElementId = model.WorkstationElementId,
                WorkstationCode = model.WorkstationCode,
                PalletNumber = model.PalletNumber,
                LocationId = model.LocationId,
                DateCreated = model.DateCreated.ToString("o"),
                CreatedBy = model.CreatedBy?.Id,
                CreatedByName = model.CreatedBy?.Name,
                Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(WorkstationElement model)
        {
            return $"/stores2/work-stations/{model.WorkstationCode}/{model.WorkstationElementId}";
        }

        object IBuilder<WorkstationElement>.Build(WorkstationElement entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(WorkstationElement model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}