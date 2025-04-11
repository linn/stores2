namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionResourceBuilder : IBuilder<StoresFunction>
    {
        private readonly IAuthorisationService authService;

        public StoresFunctionResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public StoresFunctionResource Build(StoresFunction model, IEnumerable<string> claims)
        {
            if (model == null)
            {
                return null;
            }

            return new StoresFunctionResource
                       {
                          Code = model.FunctionCode,
                          Description = model.Description,
                          FunctionType = model.FunctionType,
                          CancelFunction = model.CancelFunction,
                          DepartmentNominalRequired = model.DepartmentNominalRequired,
                          ManualPickRequired = model.ManualPickRequired,
                          FromLocationRequired = model.FromLocationRequired,
                          FromStateRequired = model.FromStateRequired,
                          FromStockPoolRequired = model.FromStockPoolRequired,
                          QuantityRequired = model.QuantityRequired,
                          ToLocationRequired = model.ToLocationRequired,
                          ToStateRequired = model.ToStateRequired,
                          ToStockPoolRequired = model.ToStockPoolRequired,
                          Document1Required = model.Document1Required(),
                          Document1Entered = model.Document1Entered(),
                          Document1Text = model.Document1Text,
                          Document1Name = model.Document1Name(),
                          Document1LineRequired = model.Document1LineRequiredFlag,
                          Document2Required = model.Document2Required(),
                          Document2Entered = model.Document2Entered(),
                          Document2Text = model.Document2Text,
                          Document2Name = model.Document2Name(),
                          FromCategory = model.FromCategory,
                          PartSource = model.PartSource,
                          PartNumberRequired = model.PartNumberRequired(),
                          BatchDateRequired = model.BatchDateRequired,
                          BatchRequired = model.BatchRequired,
                          NominalCode = model.GetNominal()?.NominalCode,
                          NominalDescription = model.GetNominal()?.Description,
                          LinesRequired = model.LinesRequired,
                          FunctionAvailable = model.FunctionAvailable(),
                          DefaultFromState = model.DefaultFromState(),
                          DefaultToState = model.DefaultToState(),
                          ToStockPool = model.ToStockPool,
                          FromStates = model.GetTransactionStates("F"),
                          ToStates = model.GetTransactionStates("O"),
                          TransactionTypes = model.TransactionsTypes?.Select(
                              t => new FunctionCodeTransactionResource
                                       {
                                           Seq = t.Seq,
                                           ReqType = t.ReqType,
                                           TransactionDefinition = t.TransactionDefinition?.TransactionCode,
                                           TransactionDescription = t.TransactionDefinition?.Description,
                                           StockAllocations = t.TransactionDefinition?.StockAllocations == "Y",
                                           FromStates = t.TransactionDefinition?.GetTransactionStates("F"),
                                           ToStates = t.TransactionDefinition?.GetTransactionStates("O")
                                       }),
                          Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(StoresFunction model)
        {
            return $"/requisitions/stores-functions/{model.FunctionCode}";
        }

        object IBuilder<StoresFunction>.Build(StoresFunction model, IEnumerable<string> claims) => this.Build(model, claims);

        private IEnumerable<LinkResource> BuildLinks(StoresFunction model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                // annoyingly not every function needs a permission
                var unAuthFunctions = new List<string>() { "LOAN OUT" };

                if (this.authService.HasPermissionFor(AuthorisedActions.GetRequisitionActionByFunction(model.FunctionCode), claims) || unAuthFunctions.Contains(model.FunctionCode))
                {
                    yield return new LinkResource { Rel = "create-req", Href = "/requisitions/create" };
                }
            }
        }
    }
}
