namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Resources.GoodsIn;

    public class GoodsInLogEntryResourceBuilder : IBuilder<GoodsInLogEntry>
    {
        public GoodsInLogEntryResource Build(GoodsInLogEntry goodsInLogEntry, IEnumerable<string> claims)
        {
            return new GoodsInLogEntryResource
                       {
                           Id = goodsInLogEntry.Id,
                           TransactionType = goodsInLogEntry.TransactionType,
                           DateCreated = goodsInLogEntry.DateCreated.ToString("o"),
                           CreatedBy = goodsInLogEntry.CreatedBy,
                           WandString = goodsInLogEntry.WandString,
                           ArticleNumber = goodsInLogEntry.ArticleNumber,
                           Quantity = goodsInLogEntry.Quantity,
                           SerialNumber = goodsInLogEntry.SerialNumber,
                           OrderNumber = goodsInLogEntry.OrderNumber,
                           OrderLine = goodsInLogEntry.OrderLine,
                           LoanNumber = goodsInLogEntry.LoanNumber,
                           LoanLine = goodsInLogEntry.LoanLine,
                           RsnNumber = goodsInLogEntry.RsnNumber,
                           ReqNumber = goodsInLogEntry.ReqNumber,
                           ReqLine = goodsInLogEntry.ReqLine,
                           SernosTref = goodsInLogEntry.SernosTref,
                           ProductAnalysisCode = goodsInLogEntry.ProductAnalysisCode,
                           StoragePlace = goodsInLogEntry.StoragePlace,
                           Processed = goodsInLogEntry.Processed,
                           ErrorMessage = goodsInLogEntry.ErrorMessage,
                           BookInRef = goodsInLogEntry.BookInRef,
                           DemLocation = goodsInLogEntry.DemLocation,
                           Comments = goodsInLogEntry.Comments,
                           State = goodsInLogEntry.State,
                           StorageType = goodsInLogEntry.StorageType,
                           ManufacturersPartNumber = goodsInLogEntry.ManufacturersPartNumber,
                           LogCondition = goodsInLogEntry.LogCondition,
                           RsnAccessories = goodsInLogEntry.RsnAccessories
                       };
        }

        public string GetLocation(GoodsInLogEntry model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<GoodsInLogEntry>.Build(GoodsInLogEntry entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);
    }
}
