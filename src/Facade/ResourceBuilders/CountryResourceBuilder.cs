namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class CountryResourceBuilder : IBuilder<Country>
    {
        public CountryResource Build(Country country, IEnumerable<string> claims)
        {
            return new CountryResource
                       {
                           CountryCode = country.CountryCode,
                           Name = country.Name,
                           Links = this.BuildLinks(country, claims).ToArray()
                       };
        }

        public string GetLocation(Country model)
        {
            return $"/stores2/countries/{model.CountryCode}";
        }

        object IBuilder<Country>.Build(Country entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Country model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}