namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Stores;

    public class WorkstationResourceBuilder : IBuilder<Workstation>
    {
        private readonly IBuilder<WorkstationElement> workstationElementsBuilder;

        public WorkstationResourceBuilder(IBuilder<WorkstationElement> workstationElementsBuilder)
        {
            this.workstationElementsBuilder = workstationElementsBuilder;
        }

        public WorkstationResource Build(Workstation model, IEnumerable<string> claims)
        {
            var claimsList = claims?.ToList() ?? new List<string>();

            return new WorkstationResource
                       {
                           WorkstationCode = model.WorkstationCode,
                           CitCode = model.Cit?.Code,
                           CitName = model.Cit?.Name,
                           Description = model.Description,
                           ZoneType = model.ZoneType,
                           WorkstationElements = model.WorkstationElements
                               ?.Select(c => (WorkstationElementResource)this.workstationElementsBuilder
                                   .Build(c, claimsList)), 
                           Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(Workstation model)
        {
            return $"/stores2/work-stations/{model.WorkstationCode}";
        }

        object IBuilder<Workstation>.Build(Workstation entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Workstation model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}