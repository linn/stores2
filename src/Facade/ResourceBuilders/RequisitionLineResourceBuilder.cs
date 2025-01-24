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
            var storesBudgetResourceBuilder = new StoresBudgetResourceBuilder();
            var reqHeaderBuilder = new RequisitionResourceBuilder();

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
                           // RequisitionHeader = l.RequisitionHeader == null
                           //                         ? null
                           //                         : reqHeaderBuilder.Build(l.RequisitionHeader, null),
                           Postings = l.NominalAccountPostings?.Select(
                               p => new RequisitionLinePostingResource
                                        {
                                            DebitOrCredit = p.DebitOrCredit,
                                            Sequence = p.Seq,
                                            DepartmentCode = p.NominalAccount?.Department?.DepartmentCode,
                                            NominalCode = p.NominalAccount?.Nominal?.NominalCode,
                                            Quantity = p.Qty,
                                            NominalAccount = nominalAccountBuilder.Build(p.NominalAccount, null)
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
