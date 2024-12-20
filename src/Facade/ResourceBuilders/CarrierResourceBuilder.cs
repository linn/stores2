namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class CarrierResourceBuilder : IBuilder<Carrier>
    {
        public CarrierResource Build(Carrier carrier, IEnumerable<string> claims)
        {
            var address = carrier.Organisation?.Address;
            return new CarrierResource
                       {
                           Code = carrier.CarrierCode,
                           Name = carrier.Name,
                           Addressee = address?.Addressee,
                           Line1 = address?.Line1,
                           Line2 = address?.Line2,
                           Line3 = address?.Line3,
                           Line4 = address?.Line4,
                           PostCode = address?.PostCode,
                           PhoneNumber = carrier.Organisation?.PhoneNumber,
                           VatRegistrationNumber = carrier.Organisation?.VatRegistrationNumber,
                           CountryCode = address?.Country?.CountryCode,
                           Links = this.BuildLinks(carrier, claims).ToArray()
                       };
        }

        public string GetLocation(Carrier model)
        {
            return $"/stores2/carriers/{model.CarrierCode}";
        }

        object IBuilder<Carrier>.Build(Carrier entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Carrier model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
