namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionResourceBuilder : IBuilder<StoresFunction>
    {
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
                          PartSource = model.PartSource,
                          PartNumberRequired = model.PartNumberRequired(),
                          BatchDateRequired = model.BatchDateRequired,
                          BatchRequired = model.BatchRequired,
                          NominalCode = model.GetNominal()?.NominalCode,
                          NominalDescription = model.GetNominal()?.Description,
                          FunctionAvailable = model.FunctionAvailable(),
                          FromStates = model.GetTransactionStates("F"),
                          ToStates = model.GetTransactionStates("O"),
                          TransactionTypes = model.TransactionsTypes?.Select(
                              t => new FunctionCodeTransactionResource
                                       {
                                           ReqType = t.ReqType,
                                           TransactionDefinition = t.TransactionDefinition?.TransactionCode,
                                           TransactionDescription = t.TransactionDefinition?.Description
                                       })
                       };
        }

        public string GetLocation(StoresFunction model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<StoresFunction>.Build(StoresFunction model, IEnumerable<string> claims) => this.Build(model, claims);
    }
}
