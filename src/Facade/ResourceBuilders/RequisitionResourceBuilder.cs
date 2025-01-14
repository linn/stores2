namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionResourceBuilder : IBuilder<RequisitionHeader>
    {
        public RequisitionHeaderResource Build(RequisitionHeader header, IEnumerable<string> claims)
        {
            return new RequisitionHeaderResource
                       {
                           ReqNumber = header.ReqNumber, DateCreated = header.DateCreated.ToString("o"), 
                           Document1 = header.Document1,
                           Qty = header.Qty,
                           Document1Name = header.Document1Name,
                           PartNumber = header.PartNumber,
                           ToLocationId = header.ToLocationId,
                           ToLocation = header.ToLocation?.LocationCode,
                           Cancelled = header.Cancelled,
                           CancelledBy = header.CancelledBy,
                           DateCancelled = header.DateCancelled?.ToString("o"),
                           CancelledReason = header.CancelledReason,
                           FunctionCode = header.FunctionCode,
                           Comments = header.Comments,
                           Lines = header.Lines?.Select(x => new RequisitionLineResource
                                                                 {
                                                                     LineNumber = x.LineNumber,
                                                                     PartNumber = x.PartNumber
                                                                 }),
                           Links = this.BuildLinks(header, claims).ToArray()
                        };
        }

        public string GetLocation(RequisitionHeader model)
        {
            return $"/stores2/requisitions/{model.ReqNumber}";
        }

        object IBuilder<RequisitionHeader>.Build(RequisitionHeader entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(RequisitionHeader model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
