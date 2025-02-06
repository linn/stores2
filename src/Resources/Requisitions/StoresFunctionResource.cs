namespace Linn.Stores2.Resources.Requisitions
{
    using System.Collections.Generic;

    public class StoresFunctionResource
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string CancelFunction { get; set; }

        public string DepartmentNominalRequired { get; set; }

        public IEnumerable<FunctionCodeTransactionResource> TransactionTypes { get; set; }
    }
}
