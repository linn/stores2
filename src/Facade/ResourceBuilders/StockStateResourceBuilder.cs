namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StockStateResourceBuilder : IBuilder<StockState>
    {
        public StockStateResource Build(StockState model, IEnumerable<string> claims)
        {
            if (model == null)
            {
                return null;
            }

            return new StockStateResource
                       {
                          State = model.State,
                          Description = model.Description,
                          QCRequired = model.QCRequired,
                          Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(StockState model)
        {
            return $"/stores2/stock/states/{model.State}";
        }

        object IBuilder<StockState>.Build(StockState model, IEnumerable<string> claims) => this.Build(model, claims);

        private IEnumerable<LinkResource> BuildLinks(StockState model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
