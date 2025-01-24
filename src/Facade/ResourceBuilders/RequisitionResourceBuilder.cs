﻿namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionResourceBuilder : IBuilder<RequisitionHeader>
    {
        public RequisitionHeaderResource Build(RequisitionHeader header, IEnumerable<string> claims)
        {
            var nominalAccountBuilder = new NominalAccountResourceBuilder();

            return new RequisitionHeaderResource
                       {
                           ReqNumber = header.ReqNumber, DateCreated = header.DateCreated.ToString("o"), 
                           Document1 = header.Document1,
                           Qty = header.Qty,
                           Document1Name = header.Document1Name,
                           PartNumber = header.PartNumber,
                           ToLocationId = header.ToLocation?.LocationId,
                           ToLocationCode = header.ToLocation?.LocationCode,
                           FromLocationCode = header.FromLocation?.LocationCode,
                           ToPalletNumber = header.ToPalletNumber,
                           FromPalletNumber = header.FromPalletNumber,
                           Cancelled = header.Cancelled,
                           CancelledBy = header.CancelledBy?.Id,
                           CancelledByName = header.CancelledBy?.Name,
                           DateCancelled = header.DateCancelled?.ToString("o"),
                           CancelledReason = header.CancelledReason,
                           FunctionCode = header.FunctionCode?.FunctionCode,
                           FunctionCodeDescription = header.FunctionCode?.Description,
                           Comments = header.Comments,
                           DateBooked = header.DateBooked?.ToString("o"),
                           BookedBy = header.BookedBy?.Id,
                           BookedByName = header.BookedBy?.Name,
                           CreatedBy = header.CreatedBy?.Id,
                           CreatedByName = header.CreatedBy?.Name,
                           Reversed = header.Reversed,
                           Lines = header
                               .Lines?.Select(
                                   l => new RequisitionLineResource 
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
                                                    LineNumber = l.LineNumber,
                                                    Seq = m.Sequence,
                                                    DateCancelled = l.DateCancelled?.ToString("o"),
                                                    DateBooked = l.DateBooked?.ToString("o"),
                                                    ReqNumber = l.ReqNumber,
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
                                                        } : null,
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
                                                        } : null
                                                })
                                            }),
                           Nominal = new NominalResource
                                         {
                                             NominalCode = header.Nominal?.NominalCode, 
                                             Description = header.Nominal?.Description
                                         },
                           Department = new DepartmentResource
                                         {
                                             DepartmentCode = header.Department?.DepartmentCode,
                                             Description = header.Department?.Description
                                         },
                           ReqType = header.ReqType,
                           ManualPick = header.ManualPick, 
                           Reference = header.Reference,
                           FromStockPool = header.FromStockPool,
                           ToStockPool = header.ToStockPool,
                           AuthorisedBy = header.AuthorisedBy?.Id,
                           AuthorisedByName = header.AuthorisedBy?.Name,
                           DateAuthorised = header.DateAuthorised?.ToString("o"),
                           Links = this.BuildLinks(header, claims).ToArray()
                        };
        }

        public string GetLocation(RequisitionHeader model)
        {
            return $"/requisitions/{model.ReqNumber}";
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
