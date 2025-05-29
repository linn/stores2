namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Pcas;

    public class PcasBoardResourceBuilder : IBuilder<PcasBoard>
    {
        public PcasBoardResource Build(PcasBoard pcasBoard, IEnumerable<string> claims)
        {
            var storageLocationResourceBuilder = new StorageLocationResourceBuilder();

            return new PcasBoardResource
            {
                BoardCode = pcasBoard.BoardCode,
                Description = pcasBoard.Description
            };
        }

        public string GetLocation(PcasBoard model)
        {
            return $"/stores2/stock-pool/{model.BoardCode}";
        }

        object IBuilder<PcasBoard>.Build(PcasBoard entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PcasBoard model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
