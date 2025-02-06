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
                          CancelFunction = model.CancelFunction,
                          DepartmentNominalRequired = model.DepartmentNominalRequired,
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
