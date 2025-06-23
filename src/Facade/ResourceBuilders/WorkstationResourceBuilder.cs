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

    public class WorkstationResourceBuilder : IBuilder<Workstation>
    {
        private readonly IBuilder<WorkstationElement> workstationElementsBuilder;

        private readonly IAuthorisationService authService;

        public WorkstationResourceBuilder(IBuilder<WorkstationElement> workstationElementsBuilder, IAuthorisationService authService)
        {
            this.workstationElementsBuilder = workstationElementsBuilder;
            this.authService = authService;
        }

        public WorkstationResource Build(Workstation model, IEnumerable<string> claims)
        {
            var claimsList = claims?.ToList() ?? new List<string>();

            return new WorkstationResource
            {
                WorkStationCode = model.WorkStationCode,
                CitCode = model.Cit?.Code,
                CitName = model.Cit?.Name,
                Description = model.Description,
                ZoneType = model.ZoneType,
                WorkStationElements = model.WorkStationElements
                               ?.Select(c => (WorkstationElementResource)this.workstationElementsBuilder
                                   .Build(c, claimsList)), 
                Links = this.BuildLinks(model, claimsList).ToArray()
            };
        }

        public string GetLocation(Workstation model)
        {
            return $"/stores2/work-stations/{model.WorkStationCode}";
        }

        object IBuilder<Workstation>.Build(Workstation entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Workstation model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }

            if (this.authService.HasPermissionFor(AuthorisedActions.WorkstationAdmin, claims))
            {
                yield return new LinkResource { Rel = "workstation-admin", Href = "/stores2/work-stations/admin" };
            }
        }
    }
}
