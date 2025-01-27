namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionLineResourceBuilder : IBuilder<RequisitionLine>
    {
        public RequisitionLineResource Build(RequisitionLine l, IEnumerable<string> claims)
        {
            var nominalAccountBuilder = new NominalAccountResourceBuilder();
            var storesBudgetResourceBuilder = new StoresBudgetResourceWithoutReqLineBuilder();

            return new RequisitionLineResource 
                       {
                           LineNumber = l.LineNumber,
                           PartNumber = l.Part?.PartNumber,
                           PartDescription = l.Part?.Description,
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
                           Moves = l.Moves?.Select(m => new MoveHeaderResource 
                                                            {
                                                                Part = l.Part?.PartNumber,
                                                                Qty = m.Quantity,
                                                                LineNumber = m.LineNumber,
                                                                Seq = m.Sequence,
                                                                DateCancelled = m.DateCancelled?.ToString("o"),
                                                                DateBooked = m.DateBooked?.ToString("o"),
                                                                ReqNumber = m.ReqNumber,
                                                                From = m.StockLocator != null ?
                                                                        new MoveFromResource
                                                                            {
                                                                                Seq = m.Sequence,
                                                                                LocationCode = m.StockLocator.StorageLocation?.LocationCode,
                                                                                State = m.StockLocator.State,
                                                                                BatchDate = m.StockLocator.StockRotationDate?.ToString("o"),
                                                                                BatchRef = m.StockLocator.BatchRef,
                                                                                PalletNumber = m.StockLocator.PalletNumber,
                                                                                LocationDescription = m.StockLocator.StorageLocation?.Description,
                                                                                QtyAllocated = m.StockLocator.QuantityAllocated,
                                                                                StockPool = m.StockLocator.StockPoolCode,
                                                                                QtyAtLocation = m.Quantity
                                                                            }
                                                                        : null,
                                                                To = m.LocationId.HasValue || m.PalletNumber.HasValue
                                                                        ? new MoveToResource
                                                                                {
                                                                                    Seq = m.Sequence,
                                                                                    LocationCode = m.Location.LocationCode,
                                                                                    LocationDescription = m.Location.Description,
                                                                                    PalletNumber = m.PalletNumber,
                                                                                    StockPool = m.StockPoolCode,
                                                                                    State = m.State,
                                                                                    SerialNumber = m.SerialNumber,
                                                                                                        Remarks = m.Remarks
                                                                                                    }
                                                                                            : null
                                                                                })
                       };
        }

        public string GetLocation(RequisitionLine model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<RequisitionLine>.Build(RequisitionLine entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);
    }
}
