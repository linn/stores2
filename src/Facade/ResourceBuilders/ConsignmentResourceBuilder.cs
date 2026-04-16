namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Resources.Consignments;

    public class ConsignmentResourceBuilder : IBuilder<Consignment>
    {
        public ConsignmentResource Build(Consignment model, IEnumerable<string> claims)
        {
            return new ConsignmentResource
            {
                Links = this.BuildLinks(model, claims).ToArray(),
                ConsignmentId = model.ConsignmentId,
                SalesAccountId = model.SalesAccountId,
                SalesAccountName = model.SalesAccount?.AccountName,
                AddressId = model.AddressId,
                DateOpened = model.DateOpened.ToString("o"),
                DateClosed = model.DateClosed?.ToString("o"),
                CustomerName = model.CustomerName,
                CarrierCode = model.CarrierCode,
                ShippingMethod = model.ShippingMethod,
                Terms = model.Terms,
                Status = model.Status,
                ClosedById = model.ClosedById,
                DespatchLocationCode = model.DespatchLocationCode,
                Warehouse = model.Warehouse,
                HubId = model.HubId,
                CustomsEntryCodePrefix = model.CustomsEntryCodePrefix,
                CustomsEntryCode = model.CustomsEntryCode,
                CustomsEntryCodeDate = model.CustomsEntryCodeDate?.ToString("o"),
                CarrierRef = model.CarrierRef,
                MasterCarrierRef = model.MasterCarrierRef
            };
        }

        public string GetLocation(Consignment model)
        {
            return $"/stores2/consignments/{model.ConsignmentId}";
        }

        object IBuilder<Consignment>.Build(Consignment model, IEnumerable<string> claims) => this.Build(model, claims);

        private IEnumerable<LinkResource> BuildLinks(Consignment model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
