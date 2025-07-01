namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Stores;

    public class WorkStationResourceBuilder : IBuilder<WorkStation>
    {
        private readonly IBuilder<WorkStationElement> workStationsElementsBuilder;

        private readonly IAuthorisationService authService;

        public WorkStationResourceBuilder(IBuilder<WorkStationElement> workstationElementsBuilder, IAuthorisationService authService)
        {
            this.workStationsElementsBuilder = workstationElementsBuilder;
            this.authService = authService;
        }

        public WorkStationResource Build(WorkStation model, IEnumerable<string> claims)
        {
            return new WorkStationResource
            {
                WorkStationCode = model.WorkStationCode,
                CitCode = model.Cit?.Code,
                CitName = model.Cit?.Name,
                Description = model.Description,
                ZoneType = model.ZoneType,
                WorkStationElements = model.WorkStationElements
                               ?.Select(c => (WorkStationElementResource)this.workStationsElementsBuilder
                                   .Build(c, claims)),
                Links = this.BuildLinks(model, claims?.ToList()).ToArray()
            };
        }

        public string GetLocation(WorkStation model)
        {
            return $"/stores2/work-stations/{model.WorkStationCode}";
        }

        object IBuilder<WorkStation>.Build(WorkStation entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(WorkStation model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }

            if (this.authService.HasPermissionFor(AuthorisedActions.WorkStationAdmin, claims))
            {
                yield return new LinkResource { Rel = "workstation.admin", Href = "/stores2/work-stations/admin" };
            }
        }
    }
}
