namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class LocationTypeResourceBuilder : IBuilder<LocationType>
    {
        public LocationTypeResource Build(LocationType locationType, IEnumerable<string> claims)
        {
            return new LocationTypeResource
            {
                 Code   = locationType.Code,
                 Description = locationType.Description,
                 Links = this.BuildLinks(locationType, claims).ToArray()
            };
        }

        public string GetLocation(LocationType model)
        {
            return $"/stores2/location-type/{model.Code}";
        }

        object IBuilder<LocationType>.Build(LocationType entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(LocationType model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
