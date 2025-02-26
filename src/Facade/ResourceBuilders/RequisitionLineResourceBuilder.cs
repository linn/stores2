namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionLineResourceBuilder : IBuilder<RequisitionLine>
    {
        public RequisitionLineResource Build(RequisitionLine l, IEnumerable<string> claims)
        {
            var nominalAccountBuilder = new NominalAccountResourceBuilder();
            var storesBudgetResourceBuilder = new StoresBudgetResourceWithoutReqLineBuilder();

            return new RequisitionLineResource 
                       {
                           ReqNumber = l.ReqNumber,
                           LineNumber = l.LineNumber,
                           Part = new PartResource
                                      {
                                          PartNumber = l.Part?.PartNumber,
                                          Description = l.Part?.Description
                                      },
                           Qty = l.Qty,
                           TransactionCode = l.TransactionDefinition?.TransactionCode,
                           TransactionCodeDescription = l.TransactionDefinition?.Description,
                           Document1Number = l.Document1Number,
                           Document1Line = l.Document1Line,
                           Document1Type = l.Document1Type,
                           Document2Number = l.Document2Number,
                           Document2Line = l.Document2Line,
                           Document2Type = l.Document2Type,
                           DateBooked = l.DateBooked?.ToString("o"),
                           Cancelled = l.Cancelled,
                           StoresBudgets = l.StoresBudgets?.Select(
                               b => storesBudgetResourceBuilder.Build(b, null)),
                           Postings = l.NominalAccountPostings?.Select(
                               p => new RequisitionLinePostingResource
                                        {
                                            DebitOrCredit = p.DebitOrCredit,
                                            Sequence = p.Seq,
                                            DepartmentCode = p.NominalAccount?.Department?.DepartmentCode,
                                            NominalCode = p.NominalAccount?.Nominal?.NominalCode,
                                            Quantity = p.Qty,
                                            NominalAccount = nominalAccountBuilder.Build(p.NominalAccount, null)
                                        }),
                           Moves = l.Moves?.Select(m => new MoveResource 
                                                            {
                                                                Part = l.Part?.PartNumber,
                                                                Qty = m.Quantity,
                                                                LineNumber = m.LineNumber,
                                                                Seq = m.Sequence,
                                                                DateCancelled = m.DateCancelled?.ToString("o"),
                                                                DateBooked = m.DateBooked?.ToString("o"),
                                                                ReqNumber = m.ReqNumber,
                                                                FromLocationCode = m.StockLocator?.StorageLocation?.LocationCode,
                                                                FromState = m.StockLocator?.State,
                                                                FromBatchDate = m.StockLocator?.StockRotationDate?.ToString("o"),
                                                                FromBatchRef = m.StockLocator?.BatchRef,
                                                                FromPalletNumber = m.StockLocator?.PalletNumber,
                                                                FromLocationDescription = m.StockLocator?.StorageLocation?.Description,
                                                                QtyAllocated = m.StockLocator?.QuantityAllocated,
                                                                FromStockPool = m.StockLocator?.StockPoolCode,
                                                                QtyAtLocation = m.Quantity,
                                                                ToLocationCode = m.Location?.LocationCode,
                                                                ToLocationDescription = m.Location?.Description,
                                                                ToPalletNumber = m.PalletNumber,
                                                                ToStockPool = m.StockPoolCode,
                                                                ToState = m.State,
                                                                SerialNumber = m.SerialNumber, 
                                                                Remarks = m.Remarks
                                                            }),
                           Links = this.BuildLinks(l).ToArray()
                       };
        }

        public string GetLocation(RequisitionLine model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<RequisitionLine>.Build(RequisitionLine entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(RequisitionLine model)
        {
            if (model.Part != null)
            {
                yield return new LinkResource("part", $"/parts/{model.Part.Id}");
            }

            if (model.OkToBook())
            {
                yield return new LinkResource("book-line", "/requisitions/book");
            }
        }
    }
}
