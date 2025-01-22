namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.Parts;
    using Linn.Stores2.Resources.Stores;

    public class StoresBudgetResourceBuilder : IBuilder<StoresBudget>
    {
        public StoresBudgetResource Build(StoresBudget storesBudget, IEnumerable<string> claims)
        {
            var reqHeaderBuilder = new RequisitionResourceBuilder();
            var nominalAccountBuilder = new NominalAccountResourceBuilder();
            var claimsList = claims?.ToList();

            return new StoresBudgetResource
                       {
                           BudgetId = storesBudget.BudgetId,
                           TransactionCode = storesBudget.TransactionCode,
                           TransactionCodeDescription = storesBudget.Transaction?.Description,
                           PartNumber = storesBudget.PartNumber,
                           Part = storesBudget.Part == null
                                      ? null
                                      : new PartResource
                                            {
                                                PartNumber = storesBudget.Part.PartNumber,
                                                Description = storesBudget.Part.Description
                                            },
                           Quantity = storesBudget.Quantity,
                           Reference = storesBudget.Reference,
                           PartPrice = storesBudget.PartPrice,
                           RequisitionNumber = storesBudget.RequisitionNumber,
                           LineNumber = storesBudget.LineNumber,
                           Requisition = storesBudget.Requisition == null
                                             ? null
                                             : reqHeaderBuilder.Build(storesBudget.Requisition, claimsList),
                           CurrencyCode = storesBudget.CurrencyCode,
                           MaterialPrice = storesBudget.MaterialPrice,
                           LabourPrice = storesBudget.LabourPrice,
                           OverheadPrice = storesBudget.OverheadPrice,
                           MachinePrice = storesBudget.MachinePrice,
                           SpotRate = storesBudget.SpotRate,
                           DateBooked = storesBudget.DateBooked?.ToString("o"),
                           StoresBudgetPostings = storesBudget.StoresBudgetPostings?.Select(a => new StoresBudgetPostingResource
                               {
                                   BudgetId = a.BudgetId,
                                   Sequence = a.Sequence,
                                   Quantity = a.Quantity,
                                   DebitOrCredit = a.DebitOrCredit,
                                   NominalAccount =
                                       a.NominalAccount == null
                                           ? null
                                           : nominalAccountBuilder.Build(a.NominalAccount, claimsList),
                                   Product = a.Product,
                                   Building = a.Building,
                                   Vehicle = a.Vehicle,
                                   Person = a.Person
                               }),
                           Links = this.BuildLinks(storesBudget, claimsList).ToArray()
            };
        }

        public string GetLocation(StoresBudget model)
        {
            return $"/stores2/budgets/{model.BudgetId}";
        }

        object IBuilder<StoresBudget>.Build(StoresBudget entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StoresBudget model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
